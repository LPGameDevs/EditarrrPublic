import { ScoresSortOptions } from "../db/scores.mjs";
import { BadRequestException, asBool, extractId } from "../utils.mjs";

const defaultPageLimit = 10;

export class ScoresApi {
    constructor(scoresDbClient, levelsDbClient) {
        this.scoresDbClient = scoresDbClient;
        this.levelsDbClient = levelsDbClient;
    }

    async postScore(levelId, requestJSON) {
        var score = requestJSON.score;
        if (!score) throw new BadRequestException(`'score' must be provided in the request.`);
        if (typeof score === "string") {
            score = score.replace(/,/g, '.');
            score = parseFloat(score);
        } 
        if (isNaN(score)) throw new BadRequestException(`'score' must be a number`);
        var scoreLevelName = requestJSON.code;
        if (!scoreLevelName) throw new BadRequestException(`'code' must be provided in the request.`);
        var scoreCreatorId = requestJSON.creator;
        if (!scoreCreatorId) throw new BadRequestException(`'creator' must be provided in the request.`);
        var scoreCreatorName = requestJSON.creatorName;
        if (!scoreCreatorName) throw new BadRequestException(`'creatorName' must be provided in the request.`);

        var level = await this.levelsDbClient.getLevel(levelId);
        if (!level) {
            throw new BadRequestException(`level ${levelId} does not exist`)
        }

        var generatedScoreId = await this.scoresDbClient.putScore(
            score, 
            levelId,
            scoreLevelName,
            scoreCreatorId,
            scoreCreatorName);

        var allScoresForLevel = await this.scoresDbClient.getAllScoresForLevel(levelId);
        
        var totalNumberOfScores = 0; 
        var sumOfAllScores = 0.0;
        for (let i = 0; i < allScoresForLevel.length; i++) {
            var dbScore = allScoresForLevel[i];

            totalNumberOfScores++;
            sumOfAllScores += dbScore.scoreNumber;
        }
        var avgScore = sumOfAllScores / allScoresForLevel.length;
        
        await this.levelsDbClient.updateLevelScoreData(levelId, avgScore, totalNumberOfScores);

        return {
            "message": `Success! Created score for: ${scoreLevelName}`,
            "id": generatedScoreId,
        }
    }

    async getPagedScores(
        levelId,
        requestSortOption,
        requestSortAsc,
        requestLimit,
        requestCursor,
        requestDedupeByUser,
    ) {

        var sortOption = ScoresSortOptions.SCORE;
        if (requestSortOption !== undefined) {
            if (!ScoresSortOptions.isValid(requestSortOption)) throw new BadRequestException(`'sort-option' must be one of: ${ScoresSortOptions.getAllValidValues()}`);
            sortOption = requestSortOption;
        }

        var sortAsc = true; // Because lowest number is the best score
        if (requestSortAsc !== undefined) {
            sortAsc = asBool(requestSortAsc);
            if (sortAsc === undefined) throw new BadRequestException(`'sort-asc' must be either 'true' or 'false'`);
        }

        var pageLimit = defaultPageLimit;
        if (requestLimit !== undefined) {
            pageLimit = parseInt(requestLimit);
            if (isNaN(pageLimit)) throw new BadRequestException(`'limit' must be a number.`);
        }

        var dedupeByUser = false;
        if (requestDedupeByUser !== undefined) {
            dedupeByUser = asBool(requestDedupeByUser);
            if (dedupeByUser == undefined) throw new BadRequestException(`'dedupe-by-user' must either be 'true' or 'false`);
            if (sortOption != ScoresSortOptions.SCORE) throw new BadRequestException(`only 'SCORE' sort option is supported if de-duping users`);
            if (requestCursor !== undefined) throw new BadRequestException(`'cursor' is not supported when de-duping users`);
        }

        var queryResponse;
        var dbScores;
        if (dedupeByUser) {
            queryResponse = await this.scoresDbClient.getAllScoresForLevel(levelId, sortAsc);
            dbScores = queryResponse;
        } else {
            queryResponse = await this.scoresDbClient.getPagedScoresForLevel(
                levelId,
                sortOption,
                sortAsc,
                pageLimit,
                requestCursor);
            dbScores = queryResponse.scores;
        }

        var responseScores = [];
        var uniqueUserNamesinTopScores = new Set();
        for (let i = 0; i < dbScores.length; i++) {

            var dbScore = dbScores[i];

            // TODO Validation

            if (dedupeByUser) {
                if (responseScores.length == pageLimit) {
                    break;
                }
                if (uniqueUserNamesinTopScores.has(dbScore.scoreCreatorName)) {
                    continue;
                } else {
                    uniqueUserNamesinTopScores.add(dbScore.scoreCreatorName);
                }
            }

            var id = extractId(dbScore.sk);
            var levelId = extractId(dbScore.pk);

            responseScores.push({
                "scoreId": id,
                "levelId": levelId,
                "score": dbScore.scoreNumber,
                "code": dbScore.scoreLevelName,
                "creator": {
                    "id": dbScore.scoreCreatorId,
                    "name": dbScore.scoreCreatorName
                },
                "submittedAt": dbScore.scoreSubmittedAt,
            });
        }

        var response = {
            scores: responseScores,
        };

        if (queryResponse.cursor) {
            response.cursor = extractId(queryResponse.cursor.sk);
        }

        return response
    }
}