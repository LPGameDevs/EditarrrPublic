// https://sinonjs.org/releases/latest/matchers/
import { assert, match, stub } from 'sinon';

import { DynamoDBDocumentClient } from '@aws-sdk/lib-dynamodb';
import { DynamoDBClient } from '@aws-sdk/client-dynamodb';

import { ScoresApi } from './scores.mjs';
import { LevelsDbClient } from '../db/levels.mjs';
import { ScoresDbClient } from '../db/scores.mjs';
import { expect } from 'chai';

var dynamoDbClient;
var ddbClientSendStub;
var scoresDbClient;
var levelsDbClient;
var scoresApi;

var tableNameScores = "editarrr-score-storage";
var tableNameLevels = "editarrr-level-storage";

var indexLevelNameScore = "scoreLevelName-score-index";

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
            ScanIndexForward: true,
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

describe('GetPagedScores', function() {
    beforeEach(function () {
        dynamoDbClient = new DynamoDBDocumentClient(new DynamoDBClient({}));
        ddbClientSendStub = stub(dynamoDbClient, 'send');
        scoresDbClient = new ScoresDbClient(dynamoDbClient);
        levelsDbClient = new LevelsDbClient(dynamoDbClient);
        scoresApi = new ScoresApi(scoresDbClient, levelsDbClient);
    });

    it('should fetch the first page of levels sorted by lowest score first & limited to 10 by default.', async function() {
        // Arrange
        var levelId = "5246cf90-7f7f-4074-aea1-ba543d27ed63";
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = undefined;
        var cursorQueryParam = undefined;
        var dedupeByUser = undefined;

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameScores,
            IndexName: indexLevelNameScore,
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 10,
            ScanIndexForward: true,
            KeyConditionExpression: "pk = :levelId",
            ExpressionAttributeValues: {
                ":levelId": "LEVEL#" + levelId
            }
        })).returns({
            "Items":[
                {
                    "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "sk": "SCORE#52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                    "score": "0.6215752",
                    "scoreLevelName": "29995",
                    "scoreSubmittedAt": 1698964301163,
                    "scoreCreatorId": "e0f86b03-7a0f-45d8-a66d-322c4e1196e5",
                    "scoreCreatorName": "Calm Pewter",
                },
                {
                    "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "sk": "SCORE#6580188b-eec0-44a9-9e56-2de2e5b9a937",
                    "score": "1.0",
                    "scoreLevelName": "29995",
                    "scoreSubmittedAt": 1698938196593,
                    "scoreCreatorId": "2cbe4992-950e-43bc-9fec-4e6138b5ce74",
                    "scoreCreatorName": "Murphys dad",
                },
            ] 
        });

        // Act
        var firstPageOfScores = await scoresApi.getPagedScores(
            levelId,
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            dedupeByUser,
        );

        // Assert
        expect(firstPageOfScores).to.deep.equal({
            "scores": [
                {
                    "scoreId": "52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                    "levelId": "5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "score": "0.6215752",
                    "code": "29995",
                    "creator": {
                        "id": "e0f86b03-7a0f-45d8-a66d-322c4e1196e5",
                        "name": "Calm Pewter"
                    },
                    "submittedAt": 1698964301163
                },
                {
                    "scoreId": "6580188b-eec0-44a9-9e56-2de2e5b9a937",
                    "levelId": "5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "score": "1.0",
                    "code": "29995",
                    "creator": {
                        "id": "2cbe4992-950e-43bc-9fec-4e6138b5ce74",
                        "name": "Murphys dad"
                    },
                    "submittedAt": 1698938196593
                },
            ]
        });
    });

    it('should fetch the first page of levels sorted by highest score first if the query option is provided.', async function() {
        // Arrange
        var levelId = "5246cf90-7f7f-4074-aea1-ba543d27ed63";
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = "false";
        var limitQueryParam = undefined;
        var cursorQueryParam = undefined;
        var dedupeByUser = undefined;

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameScores,
            IndexName: indexLevelNameScore,
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 10,
            ScanIndexForward: false,
            KeyConditionExpression: "pk = :levelId",
            ExpressionAttributeValues: {
                ":levelId": "LEVEL#" + levelId
            }
        })).returns({
            "Items":[
                {
                    "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "sk": "SCORE#6580188b-eec0-44a9-9e56-2de2e5b9a937",
                    "score": "1.0",
                    "scoreLevelName": "29995",
                    "scoreSubmittedAt": 1698938196593,
                    "scoreCreatorId": "2cbe4992-950e-43bc-9fec-4e6138b5ce74",
                    "scoreCreatorName": "Murphys dad",
                },
                {
                    "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "sk": "SCORE#52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                    "score": "0.6215752",
                    "scoreLevelName": "29995",
                    "scoreSubmittedAt": 1698964301163,
                    "scoreCreatorId": "e0f86b03-7a0f-45d8-a66d-322c4e1196e5",
                    "scoreCreatorName": "Calm Pewter",
                },
            ] 
        });

        // Act
        var firstPageOfScores = await scoresApi.getPagedScores(
            levelId,
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            dedupeByUser,
        );

        // Assert
        expect(firstPageOfScores).to.deep.equal({
            "scores": [
                {
                    "scoreId": "6580188b-eec0-44a9-9e56-2de2e5b9a937",
                    "levelId": "5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "score": "1.0",
                    "code": "29995",
                    "creator": {
                        "id": "2cbe4992-950e-43bc-9fec-4e6138b5ce74",
                        "name": "Murphys dad"
                    },
                    "submittedAt": 1698938196593
                },
                {
                    "scoreId": "52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                    "levelId": "5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "score": "0.6215752",
                    "code": "29995",
                    "creator": {
                        "id": "e0f86b03-7a0f-45d8-a66d-322c4e1196e5",
                        "name": "Calm Pewter"
                    },
                    "submittedAt": 1698964301163
                },
            ]
        });
    });

    it('should fetch the first page of levels and return a cursor if there are more.', async function() {
        // Arrange
        var levelId = "5246cf90-7f7f-4074-aea1-ba543d27ed63";
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = "1";
        var cursorQueryParam = undefined;
        var dedupeByUser = undefined;

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameScores,
            IndexName: indexLevelNameScore,
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 1,
            ScanIndexForward: true,
            KeyConditionExpression: "pk = :levelId",
            ExpressionAttributeValues: {
                ":levelId": "LEVEL#" + levelId
            }
        })).returns({
            "Items":[
                {
                    "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "sk": "SCORE#52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                    "score": "0.6215752",
                    "scoreLevelName": "29995",
                    "scoreSubmittedAt": 1698964301163,
                    "scoreCreatorId": "e0f86b03-7a0f-45d8-a66d-322c4e1196e5",
                    "scoreCreatorName": "Calm Pewter",
                },
            ],
            "LastEvaluatedKey": {
                "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                "sk": "SCORE#52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                "score": "0.6215752",
            },
        });

        // Act
        var firstPageOfScores = await scoresApi.getPagedScores(
            levelId,
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            dedupeByUser,
        );

        // Assert
        expect(firstPageOfScores).to.deep.equal({
            "scores": [
                {
                    "scoreId": "52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                    "levelId": "5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "score": "0.6215752",
                    "code": "29995",
                    "creator": {
                        "id": "e0f86b03-7a0f-45d8-a66d-322c4e1196e5",
                        "name": "Calm Pewter"
                    },
                    "submittedAt": 1698964301163
                },
            ],
            "cursor": "52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
        });
    });

    it('should fetch the second page of levels if a cursor is provided.', async function() {
        // Arrange
        var levelId = "5246cf90-7f7f-4074-aea1-ba543d27ed63";
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = undefined;
        var cursorQueryParam = "52dff6ed-eb42-4ec1-81d6-d36db8d048d6";
        var dedupeByUser = undefined;

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameScores,
            Key: {
                pk: `SCORE#${cursorQueryParam}`,
                sk: `SCORE#${cursorQueryParam}`,
            }
        })).returns({
            "Item": {
                "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                "sk": "SCORE#52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                "score": "0.6215752",
                "scoreLevelName": "29995",
                "scoreSubmittedAt": 1698964301163,
                "scoreCreatorId": "e0f86b03-7a0f-45d8-a66d-322c4e1196e5",
                "scoreCreatorName": "Calm Pewter",
            },
        });

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameScores,
            IndexName: indexLevelNameScore,
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 10,
            ScanIndexForward: true,
            KeyConditionExpression: "pk = :levelId",
            ExpressionAttributeValues: {
                ":levelId": "LEVEL#" + levelId
            },
            ExclusiveStartKey: {
                "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                "sk": "SCORE#52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                "score": "0.6215752",
            },
        })).returns({
            "Items":[
                {
                    "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "sk": "SCORE#6580188b-eec0-44a9-9e56-2de2e5b9a937",
                    "score": "1.0",
                    "scoreLevelName": "29995",
                    "scoreSubmittedAt": 1698938196593,
                    "scoreCreatorId": "2cbe4992-950e-43bc-9fec-4e6138b5ce74",
                    "scoreCreatorName": "Murphys dad",
                },
            ] 
        });

        // Act
        var secondPageOfScores = await scoresApi.getPagedScores(
            levelId,
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            dedupeByUser,
        );

        // Assert
        expect(secondPageOfScores).to.deep.equal({
            "scores": [
                {
                    "scoreId": "6580188b-eec0-44a9-9e56-2de2e5b9a937",
                    "levelId": "5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "score": "1.0",
                    "code": "29995",
                    "creator": {
                        "id": "2cbe4992-950e-43bc-9fec-4e6138b5ce74",
                        "name": "Murphys dad"
                    },
                    "submittedAt": 1698938196593
                },
            ],
        });
    });

    it('should de-dupe by username if requested, returning only the top score (based on the sort order).', async function() {
        // Arrange
        var levelId = "5246cf90-7f7f-4074-aea1-ba543d27ed63";
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = "1";
        var cursorQueryParam = undefined;
        var dedupeByUser = "true";

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameScores,
            IndexName: indexLevelNameScore,
            Select: "ALL_PROJECTED_ATTRIBUTES",
            ScanIndexForward: true,
            KeyConditionExpression: "pk = :levelId",
            ExpressionAttributeValues: {
                ":levelId": "LEVEL#" + levelId
            }
        })).returns({
            "Items":[
                {
                    "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "sk": "SCORE#52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                    "score": "0.6215752",
                    "scoreLevelName": "29995",
                    "scoreSubmittedAt": 1698964301163,
                    "scoreCreatorId": "e0f86b03-7a0f-45d8-a66d-322c4e1196e5",
                    "scoreCreatorName": "Calm Pewter",
                },
                {
                    "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "sk": "SCORE#6580188b-eec0-44a9-9e56-2de2e5b9a937",
                    "score": "1.0",
                    "scoreLevelName": "29995",
                    "scoreSubmittedAt": 1698938196593,
                    "scoreCreatorId": "2cbe4992-950e-43bc-9fec-4e6138b5ce74",
                    "scoreCreatorName": "Murphys dad",
                },
                {
                    "pk": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "sk": "SCORE#6580188b-eec0-44a9-9e56-2de2e5b9a937",
                    "score": "1.0",
                    "scoreLevelName": "29995",
                    "scoreSubmittedAt": 1698938196593,
                    "scoreCreatorId": "2cbe4992-950e-43bc-9fec-4e6138b5ce74",
                    "scoreCreatorName": "Murphys dad",
                },
            ] 
        });

        // Act
        var firstPageOfScores = await scoresApi.getPagedScores(
            levelId,
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            dedupeByUser,
        );

        // Assert
        expect(firstPageOfScores).to.deep.equal({
            "scores": [
                {
                    "scoreId": "52dff6ed-eb42-4ec1-81d6-d36db8d048d6",
                    "levelId": "5246cf90-7f7f-4074-aea1-ba543d27ed63",
                    "score": "0.6215752",
                    "code": "29995",
                    "creator": {
                        "id": "e0f86b03-7a0f-45d8-a66d-322c4e1196e5",
                        "name": "Calm Pewter"
                    },
                    "submittedAt": 1698964301163
                },
            ]
        });
    });
});