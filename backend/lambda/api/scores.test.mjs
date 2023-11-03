// https://sinonjs.org/releases/latest/matchers/
import { assert, match, stub } from 'sinon';

import { DynamoDBDocumentClient } from '@aws-sdk/lib-dynamodb';
import { DynamoDBClient } from '@aws-sdk/client-dynamodb';

import { ScoresApi } from './scores.mjs';
import { LevelsDbClient } from '../db/levels.mjs';
import { ScoresDbClient } from '../db/scores.mjs';

var dynamoDbClient;
var ddbClientSendStub;
var scoresDbClient;
var levelsDbClient;
var scoresApi;

var tableNameScores = "editarrr-score-storage"
var tableNameLevels = "editarrr-level-storage"

describe('PostScore', function () {
    
    beforeEach(function () {
        dynamoDbClient = new DynamoDBDocumentClient(new DynamoDBClient({}));
        ddbClientSendStub = stub(dynamoDbClient, 'send');
        scoresDbClient = new ScoresDbClient(dynamoDbClient);
        levelsDbClient = new LevelsDbClient(dynamoDbClient);
        scoresApi = new ScoresApi(scoresDbClient, levelsDbClient);
    });

    it('should check that the level exists, add the score, then calculate and store the avg & total number of scores."', async function () {
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
            TableName: tableNameScores,
            IndexName: "scoreLevelName-score-index",
            // TODO can we do a subset of attributes?
            Select: "ALL_PROJECTED_ATTRIBUTES",
            KeyConditionExpression: "pk = :levelId",
            ExpressionAttributeValues: {
                ":levelId": `LEVEL#${levelId}`,
            }
        })).returns({
            "Items": [
                {
                    score: 1.0
                },
                {
                    score: 3.0
                }
            ]
        });
        
        // Act
        await scoresApi.postScore(levelId, {
            "score": 1.0, 
            "code": levelName,
            "creator": levelCreatorId,
            "creatorName": levelCreatorId,
        });

        // Assert
        assert.calledWith(ddbClientSendStub, match.has("input", 
            match.has("TableName", tableNameScores).and(
            match.has("Item", 
                match.has("pk", `LEVEL#${levelId}`).and(
                match.has("score", 1.0)),
        ))));
        assert.calledWith(ddbClientSendStub, match.has("input", {
            TableName: tableNameLevels,
            Key: {
                pk: `LEVEL#${levelId}`,
                sk: `LEVEL#${levelId}`
            },
            UpdateExpression: "SET levelAvgScore = :avgScore, levelTotalScores = :totalScores",
            ExpressionAttributeValues: {
              ":avgScore": 2.0,
              ":totalScores": 2,
            },
        }));
    });
});