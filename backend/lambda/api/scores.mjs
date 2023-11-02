import { BadRequestException } from "../utils.mjs";

export class ScoresApi {
    constructor(scoresDbClient, levelsDbClient) {
        this.scoresDbClient = scoresDbClient;
        this.levelsDbClient = levelsDbClient;
    }

    async postScore(levelId, requestJSON) {
        var score = requestJSON.score;
        if (!score) throw new BadRequestException(`'score' must be provided in the request.`);
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
            sumOfAllScores += parseFloat(dbScore.score);
        }
        var avgScore = sumOfAllScores / allScoresForLevel.length;
        
        await this.levelsDbClient.updateLevelScoreData(levelId, avgScore, totalNumberOfScores);

        return {
            "message": `Success! Created score for: ${scoreLevelName}`,
            "id": generatedScoreId,
        }
    }
}