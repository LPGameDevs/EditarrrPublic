// https://sinonjs.org/releases/latest/matchers/
import { assert, match, stub } from 'sinon';

import { DynamoDBDocumentClient } from '@aws-sdk/lib-dynamodb';
import { DynamoDBClient } from '@aws-sdk/client-dynamodb';

import { RatingsApi } from './ratings.mjs';
import { LevelsDbClient } from '../db/levels.mjs';
import { RatingsDbClient } from '../db/ratings.mjs';

var dynamoDbClient;
var ddbClientSendStub;
var ratingsDbClient;
var levelsDbClient;
var ratingsApi;

var tableNameRatings = "editarrr-rating-storage"
var tableNameLevels = "editarrr-level-storage"

var indexLevelIdRating = "pk-rating-index"

describe('PostRating', function () {
    
    beforeEach(function () {
        dynamoDbClient = new DynamoDBDocumentClient(new DynamoDBClient({}));
        ddbClientSendStub = stub(dynamoDbClient, 'send');
        ratingsDbClient = new RatingsDbClient(dynamoDbClient);
        levelsDbClient = new LevelsDbClient(dynamoDbClient);
        ratingsApi = new RatingsApi(ratingsDbClient, levelsDbClient);
    });

    it('should check that the level exists, add the rating, then calculate and store the avg & total number of ratings."', async function () {
        // Arrange
        var levelId = "murphys-dads-level-id";
        var levelName = "Murphys Dads Level";
        var levelCreatorId = "murphys-dads-id";
        var levelCreatorName = "Murphys Dad";
        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevels,
            Key: {
                pk: `LEVEL#${levelId}`,
                sk: `LEVEL#${levelId}`,
            }
        })).returns({
            "Item": {
                "levelName": levelName,
                "levelUpdatedAt": 1698515874383,
                "levelCreatorId": levelCreatorId,
                "levelData": {},
                "levelStatus": "PUBLISHED",
                "sk": `LEVEL#${levelId}`,
                "levelCreatedAt": 1698514557729,
                "pk": `LEVEL#${levelId}`,
                "levelCreatorName": levelCreatorName,
            }
        });

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameRatings,
            IndexName: indexLevelIdRating,
            Select: "ALL_PROJECTED_ATTRIBUTES",
            KeyConditionExpression: "pk = :levelId",
            ExpressionAttributeValues: {
                ":levelId": `LEVEL#${levelId}`,
            }
        })).returns({
            "Items": [
                {
                    rating: 1
                },
                {
                    rating: 3
                }
            ]
        });
        
        // Act
        await ratingsApi.postRating(levelId, {
            "rating": 1, 
            "code": levelName,
            "creator": levelCreatorId,
            "creatorName": levelCreatorId,
        });

        // Assert
        assert.calledWith(ddbClientSendStub, match.has("input", 
            match.has("TableName", tableNameRatings).and(
            match.has("Item", 
                match.has("pk", `LEVEL#${levelId}`).and(
                match.has("rating", 1)),
        ))));
        assert.calledWith(ddbClientSendStub, match.has("input", {
            TableName: tableNameLevels,
            Key: {
                pk: `LEVEL#${levelId}`,
                sk: `LEVEL#${levelId}`
            },
            UpdateExpression: "SET levelAvgRating = :avgRating, levelTotalRatings = :totalRatings",
            ExpressionAttributeValues: {
              ":avgRating": 2,
              ":totalRatings": 2,
            },
        }));
    });
});