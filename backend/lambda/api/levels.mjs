import { extractLevelId } from "../utils.mjs";

export class LevelsApi {
    constructor(levelsDbClient) {
        this.levelsDbClient = levelsDbClient;
    }
    
    async getPagedLevels(pageLimit, cursor) {
        // TODO
    }

    async getLevel(levelId) {
        var dbLevel = await this.levelsDbClient.getLevel(levelId);

        // TODO Validation of queried response

        // TODO Refactor into a separate function
        var id = extractLevelId(dbLevel.pk);

        // TODO Include avg and total ratings & scores
        return {
            "id": id,
            "name": dbLevel.levelName,
            "creator": {
                "id": dbLevel.levelCreatorId,
                "name": dbLevel.levelCreatorName
            },
            "status": dbLevel.levelStatus,
            "createdAt": dbLevel.levelCreatedAt,
            "updatedAt": dbLevel.levelUpdatedAt,
            "data": dbLevel.levelData
        }
    }
}