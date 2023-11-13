import { LevelsSortOptions } from "../db/levels.mjs";
import { BadRequestException, asBool, extractId } from "../utils.mjs";

const defaultPageLimit = 10;

export class LevelsApi {
    constructor(levelsDbClient) {
        this.levelsDbClient = levelsDbClient;
    }

    async getPagedLevels(
        requestSortOption,
        requestSortAsc,
        requestLimit,
        requestCursor,
        requestUseDrafts,
    ) {
        var sortOption = LevelsSortOptions.UPDATED_AT;
        if (requestSortOption !== undefined) {
            if (!LevelsSortOptions.isValid(requestSortOption)) throw new BadRequestException(`'sort-option' must be one of: ${LevelsSortOptions.getAllValidValues()}`);
            sortOption = requestSortOption;
        }

        var sortAsc = false;
        if (requestSortAsc !== undefined) {
            sortAsc = asBool(requestSortAsc);
            if (sortAsc === undefined) throw new BadRequestException(`'sort-asc' must be either 'true' or 'false'`);
        }

        var pageLimit = defaultPageLimit;
        if (requestLimit !== undefined) {
            pageLimit = parseInt(requestLimit);
            if (isNaN(pageLimit)) throw new BadRequestException(`'limit' must be a number.`);
        }

        var useDrafts = false;
        if (requestUseDrafts !== undefined) {
            useDrafts = asBool(requestUseDrafts);
            if (useDrafts === undefined) throw new BadRequestException(`'draft' must be either 'true' or 'false'`);
        }

        var queryResponse = await this.levelsDbClient.getPagedLevels(
            sortOption,
            sortAsc,
            pageLimit,
            requestCursor,
            useDrafts);

        var dbLevels = queryResponse.levels;
        var responseLevels = [];
        for (let i = 0; i < dbLevels.length; i++) {
            var dbLevel = dbLevels[i];

            // TODO Validation

            responseLevels.push(asApiLevel(dbLevel));
        }

        var response = {
            levels: responseLevels
        }

        if (queryResponse.cursor) {
            response.cursor = extractId(queryResponse.cursor.pk);
        }

        return response
    }

    async getLevel(levelId) {
        var dbLevel = await this.levelsDbClient.getLevel(levelId);

        // TODO Validation of queried response

        return asApiLevel(dbLevel);
    }
}

function asApiLevel(dbLevel) {
    var id = extractId(dbLevel.pk);

    var apiLevel = {
        "id": id,
        "name": dbLevel.levelName,
        "creator": {
            "id": dbLevel.levelCreatorId,
            "name": dbLevel.levelCreatorName
        },
        "labels": dbLevel.labels ?? [],
        "status": dbLevel.levelStatus,
        "createdAt": dbLevel.levelCreatedAt,
        "updatedAt": dbLevel.levelUpdatedAt,
        "totalRatings": dbLevel.levelTotalRatings ?? 0,
        "totalScores": dbLevel.levelTotalScores ?? 0,
    };

    if (dbLevel.levelData) {
        apiLevel.data = dbLevel.levelData;
    }
    if (dbLevel.version) {
        apiLevel.version = dbLevel.version;
    }
    if (dbLevel.levelAvgScore) {
        apiLevel.avgScore = dbLevel.levelAvgScore;
    }
    if (dbLevel.levelAvgRating) {
        apiLevel.avgRating = dbLevel.levelAvgRating;
    }

    return apiLevel
}
