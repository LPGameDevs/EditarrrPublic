import { expect, use } from 'chai';
import { match, stub } from 'sinon';

import { DynamoDBDocumentClient } from '@aws-sdk/lib-dynamodb';
import { DynamoDBClient } from '@aws-sdk/client-dynamodb';

import { LevelsApi } from './levels.mjs';
import { LevelsDbClient } from '../db/levels.mjs';
import { BadRequestException } from "../utils.mjs";

var dynamoDbClient;
var ddbClientSendStub;
var levelsDbClient;
var levelsApi;

const tableNameLevel = "editarrr-level-storage";

describe('GetLevel', function () {
    
    beforeEach(function () {
        dynamoDbClient = new DynamoDBDocumentClient(new DynamoDBClient({}));
        ddbClientSendStub = stub(dynamoDbClient, 'send');
        levelsDbClient = new LevelsDbClient(dynamoDbClient);
        levelsApi = new LevelsApi(levelsDbClient);
    });

    it('should fetch and translate a level from the db.', async function () {
        // Arrange
        const levelId = "4a414edd-da39-4412-8dda-cc484c77966c";
        
        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            Key: {
                pk: `LEVEL#${levelId}`,
                sk: `LEVEL#${levelId}`,
            }
        })).returns({
            "Item": {
                "levelName": "65758",
                "levelUpdatedAt": 1698515874383,
                "levelCreatorId": "0f785537-b000-4b4f-a349-6ac781460681",
                "levelData": {},
                "levelStatus": "PUBLISHED",
                "sk": "LEVEL#4a414edd-da39-4412-8dda-cc484c77966c",
                "levelCreatedAt": 1698514557729,
                "pk": "LEVEL#4a414edd-da39-4412-8dda-cc484c77966c",
                "levelCreatorName": "Fresch",
                "levelAvgScore": 1.0,
                "levelTotalScores": 1,
                "levelAvgRating": 1,
                "levelTotalRatings": 1,
            }
        });
        
        // Act
        const result = await levelsApi.getLevel(levelId);

        // Assert
        expect(result).to.deep.equal({
            "id": "4a414edd-da39-4412-8dda-cc484c77966c",
            "name": "65758",
            "creator": {
                "id": "0f785537-b000-4b4f-a349-6ac781460681",
                "name": "Fresch",
            },
            "updatedAt": 1698515874383,
            "data": {},
            "status": "PUBLISHED",
            "createdAt": 1698514557729,
            "avgScore": 1.0,
            "totalScores": 1,
            "avgRating": 1,
            "totalRatings": 1,
            "labels": [],
        });
    });
});

describe('GetPagedLevels', function () {
    
    beforeEach(function () {
        dynamoDbClient = new DynamoDBDocumentClient(new DynamoDBClient({}));
        ddbClientSendStub = stub(dynamoDbClient, 'send');
        levelsDbClient = new LevelsDbClient(dynamoDbClient);
        levelsApi = new LevelsApi(levelsDbClient);
    });

    it('should throw a BadRequestException if arrayOfLabels is NOT a list of strings.', async function () {
        // Arrange
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = undefined;
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = undefined;
        var filters = {
            anyOfLabels: "foobar",
        }

        try {
            // Act
            await levelsApi.getPagedLevels(
                sortOptionQueryParam,
                sortAscQueryParam,
                limitQueryParam,
                cursorQueryParam,
                useDraftsQueryParam,
                filters,
            );

            expect.fail('Expected to throw BadRequestException but did not throw any exception.');
        } catch (error) {
            // Assert
            expect(error).to.be.an.instanceOf(BadRequestException);
            expect(error.message).to.equal("error - Bad Request: 'anyOfLabels' must be a comma-separated list of strings. E.g. 'anyOfLabels=test,GDFG'");
        }
    });

    it('should throw a BadRequestException if nameContains is NOT a string.', async function () {
        // Arrange
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = undefined;
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = undefined;
        var filters = {
            nameContains: 123,
        }

        try {
            // Act
            await levelsApi.getPagedLevels(
                sortOptionQueryParam,
                sortAscQueryParam,
                limitQueryParam,
                cursorQueryParam,
                useDraftsQueryParam,
                filters,
            );
            
            expect.fail('Expected to throw BadRequestException but did not throw any exception.');
        } catch (error) {
            // Assert
            expect(error).to.be.an.instanceOf(BadRequestException);
            expect(error.message).to.equal("error - Bad Request: 'nameContains' must be a string");
        }
    });

    it('should fetch the first page of levels sorted by last updated.', async function () {
        // Arrange
        var sortOptionQueryParam = "updated-at";
        var sortAscQueryParam = "false";
        var limitQueryParam = "2";
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = "false";

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelUpdatedAt-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 2,
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            ExpressionAttributeValues: { ":status": "PUBLISHED" },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 2,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                    "labels": [],
                },
                {
                    "levelName": "Level 1",
                    "levelUpdatedAt": 1,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#1-1-1-1-1",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#1-1-1-1-1",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                    "labels": [],
                },
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 2,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                },
                {
                    "id": "1-1-1-1-1",
                    "name": "Level 1",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                }
            ]   
        });
    });

    it('should fetch the first page of levels sorted by most recently created.', async function () {
        // Arrange
        var sortOptionQueryParam = "created-at";
        var sortAscQueryParam = "false";
        var limitQueryParam = "2";
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = "false";

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelCreatedAt-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 2,
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            ExpressionAttributeValues: { ":status": "PUBLISHED" },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 2,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
                {
                    "levelName": "Level 1",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#1-1-1-1-1",
                    "levelCreatedAt": 1,
                    "pk": "LEVEL#1-1-1-1-1",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 2,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                },
                {
                    "id": "1-1-1-1-1",
                    "name": "Level 1",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                }
            ]   
        });
    });

    it('should fetch the first page of levels sorted by average score.', async function () {
        // Arrange
        var sortOptionQueryParam = "avg-score";
        var sortAscQueryParam = "false";
        var limitQueryParam = "2";
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = "false";

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelAvgScore-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 2,
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            ExpressionAttributeValues: { ":status": "PUBLISHED" },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 2.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
                {
                    "levelName": "Level 1",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#1-1-1-1-1",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#1-1-1-1-1",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 2.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                },
                {
                    "id": "1-1-1-1-1",
                    "name": "Level 1",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                }
            ]   
        });
    });

    it('should fetch the first page of levels sorted by total number of scores.', async function () {
        // Arrange
        var sortOptionQueryParam = "total-scores";
        var sortAscQueryParam = "false";
        var limitQueryParam = "2";
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = "false";

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelTotalScores-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 2,
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            ExpressionAttributeValues: { ":status": "PUBLISHED" },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
                {
                    "levelName": "Level 1",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#1-1-1-1-1",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#1-1-1-1-1",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 1,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                },
                {
                    "id": "1-1-1-1-1",
                    "name": "Level 1",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 1,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                }
            ]   
        });
    });

    it('should fetch the first page of levels sorted by average rating.', async function () {
        // Arrange
        var sortOptionQueryParam = "avg-rating";
        var sortAscQueryParam = "false";
        var limitQueryParam = "2";
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = "false";

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelAvgRating-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 2,
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            ExpressionAttributeValues: { ":status": "PUBLISHED" },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 2,
                    "levelTotalRatings": 2,
                },
                {
                    "levelName": "Level 1",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#1-1-1-1-1",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#1-1-1-1-1",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 2,
                    "totalRatings": 2,
                    "labels": [],
                },
                {
                    "id": "1-1-1-1-1",
                    "name": "Level 1",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                }
            ]   
        });
    });

    it('should fetch the first page of levels sorted by total number of ratings.', async function () {
        // Arrange
        var sortOptionQueryParam = "total-ratings";
        var sortAscQueryParam = "false";
        var limitQueryParam = "2";
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = "false";

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelTotalRatings-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 2,
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            ExpressionAttributeValues: { ":status": "PUBLISHED" },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
                {
                    "levelName": "Level 1",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#1-1-1-1-1",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#1-1-1-1-1",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 1,
                },
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                },
                {
                    "id": "1-1-1-1-1",
                    "name": "Level 1",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 1,
                    "labels": [],
                }
            ]   
        });
    });

    it('should fetch the first page of levels sorted by level code.', async function () {
        // Arrange
        var sortOptionQueryParam = "level-code";
        var sortAscQueryParam = "false";
        var limitQueryParam = "2";
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = "false";

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelName-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 2,
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            ExpressionAttributeValues: { ":status": "PUBLISHED" },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
                {
                    "levelName": "Level 1",
                    "levelUpdatedAt": 1698514557729,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#1-1-1-1-1",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#1-1-1-1-1",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 1,
                },
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                },
                {
                    "id": "1-1-1-1-1",
                    "name": "Level 1",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1698514557729,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 1,
                    "labels": [],
                }
            ]   
        });
    });

    it('should fetch the first page of levels, and return a cursor if there are more.', async function () {
        // Arrange
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = "1";
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = undefined;

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelUpdatedAt-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 1,
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            ExpressionAttributeValues: { ":status": "PUBLISHED" },
        })).returns({
            Items:[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 2,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
            ],
            LastEvaluatedKey: {
                "levelUpdatedAt": 2,
                "levelStatus": "PUBLISHED",
                "sk": "LEVEL#2-2-2-2-2",
                "pk": "LEVEL#2-2-2-2-2",
            }
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 2,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                }
            ],
            cursor: "2-2-2-2-2",
        });
    });

    it('should fetch the second page of levels if a cursor is provided.', async function () {
        // Arrange
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = "1";
        var cursorQueryParam = "2-2-2-2-2";
        var useDraftsQueryParam = undefined;

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            Key: {
                pk: `LEVEL#${cursorQueryParam}`,
                sk: `LEVEL#${cursorQueryParam}`,
            }
        })).returns({
            "Item": {
                "levelName": "Level 2",
                "levelUpdatedAt": 2,
                "levelCreatorId": "user-1-id",
                "levelData": {},
                "levelStatus": "PUBLISHED",
                "sk": "LEVEL#2-2-2-2-2",
                "levelCreatedAt": 1698514557729,
                "pk": "LEVEL#2-2-2-2-2",
                "levelCreatorName": "User 1",
                "levelAvgScore": 1.0,
                "levelTotalScores": 2,
                "levelAvgRating": 1,
                "levelTotalRatings": 2,
            }
        });

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelUpdatedAt-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: 1,
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            ExpressionAttributeValues: { ":status": "PUBLISHED" },
            ExclusiveStartKey: {
                "levelUpdatedAt": 2,
                "levelStatus": "PUBLISHED",
                "sk": "LEVEL#2-2-2-2-2",
                "pk": "LEVEL#2-2-2-2-2",
            }
        })).returns({
            Items:[
                {
                    "levelName": "Level 1",
                    "levelUpdatedAt": 1,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#1-1-1-1-1",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#1-1-1-1-1",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                },
            ]
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "1-1-1-1-1",
                    "name": "Level 1",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [],
                }
            ],
        });
    });

    it('should fetch levels filtered by labels if anyOfLabels is set.', async function () {
        // Arrange
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = undefined;
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = undefined;
        var filters = {
            anyOfLabels: [ "test", "GDFG" ],
        }

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelUpdatedAt-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            FilterExpression: "contains(labels, :label0) OR contains(labels, :label1)",
            ExpressionAttributeValues: { 
                ":status": "PUBLISHED",
                ":label0": "test",
                ":label1": "GDFG",
             },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 2,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                    "labels": [ "test" ],
                },
                {
                    "levelName": "Level 1",
                    "levelUpdatedAt": 1,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#1-1-1-1-1",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#1-1-1-1-1",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                    "labels": [ "GDFG" ],
                },
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
            filters,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 2,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [ "test" ],
                },
                {
                    "id": "1-1-1-1-1",
                    "name": "Level 1",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [ "GDFG" ],
                }
            ]    
        });
    });

    it('should fetch levels filtered by names with matching substrings if nameContains is set.', async function () {
        // Arrange
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = undefined;
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = undefined;
        var filters = {
            nameContains: "2",
        }

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelUpdatedAt-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            FilterExpression: "contains(levelName, :nameSubstring)",
            ExpressionAttributeValues: { 
                ":status": "PUBLISHED",
                ":nameSubstring": "2",
             },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 234",
                    "levelUpdatedAt": 2,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                    "labels": [ "test" ],
                },
                {
                    "levelName": "Level 123",
                    "levelUpdatedAt": 1,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#1-1-1-1-1",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#1-1-1-1-1",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                    "labels": [ "GDFG" ],
                },
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
            filters,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 234",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 2,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [ "test" ],
                },
                {
                    "id": "1-1-1-1-1",
                    "name": "Level 123",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 1,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [ "GDFG" ],
                }
            ]    
        });
    });

    it('should fetch levels filtered by labels if anyOfLabels is set AND filtered by names with matching substrings if nameContains is set.', async function () {
        // Arrange
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = undefined;
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = undefined;
        var filters = {
            anyOfLabels: [ "test", "GDFG" ],
            nameContains: "2",
        }

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelUpdatedAt-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            FilterExpression: "(contains(labels, :label0) OR contains(labels, :label1)) AND (contains(levelName, :nameSubstring))",
            ExpressionAttributeValues: { 
                ":status": "PUBLISHED",
                ":label0": "test",
                ":label1": "GDFG",
                ":nameSubstring": "2",
             },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 2,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                    "labels": [ "test" ],
                }
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
            filters,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 2,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [ "test" ],
                }
            ]    
        });
    });

    it('should still paginate (artifically) when label and/or nameContains filters are set.', async function () {
        // Arrange
        var sortOptionQueryParam = undefined;
        var sortAscQueryParam = undefined;
        var limitQueryParam = "1";
        var cursorQueryParam = undefined;
        var useDraftsQueryParam = undefined;
        var filters = {
            anyOfLabels: [ "test", "GDFG" ],
            nameContains: "2",
        }

        ddbClientSendStub.withArgs(match.has("input", {
            TableName: tableNameLevel,
            IndexName: "levelStatus-levelUpdatedAt-index",
            Select: "ALL_PROJECTED_ATTRIBUTES",
            ScanIndexForward: false,
            KeyConditionExpression: "levelStatus = :status",
            FilterExpression: "(contains(labels, :label0) OR contains(labels, :label1)) AND (contains(levelName, :nameSubstring))",
            ExpressionAttributeValues: { 
                ":status": "PUBLISHED",
                ":label0": "test",
                ":label1": "GDFG",
                ":nameSubstring": "2",
             },
        })).returns({
            "Items":[
                {
                    "levelName": "Level 2",
                    "levelUpdatedAt": 2,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#2-2-2-2-2",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#2-2-2-2-2",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                    "labels": [ "test" ],
                },
                {
                    "levelName": "Level 22",
                    "levelUpdatedAt": 22,
                    "levelCreatorId": "user-1-id",
                    "levelData": {},
                    "levelStatus": "PUBLISHED",
                    "sk": "LEVEL#22-22-22-22-22",
                    "levelCreatedAt": 1698514557729,
                    "pk": "LEVEL#22-22-22-22-22",
                    "levelCreatorName": "User 1",
                    "levelAvgScore": 1.0,
                    "levelTotalScores": 2,
                    "levelAvgRating": 1,
                    "levelTotalRatings": 2,
                    "labels": [ "GDFG" ],
                }
            ] 
        });


        // Act
        var firstPageOfLevels = await levelsApi.getPagedLevels(
            sortOptionQueryParam,
            sortAscQueryParam,
            limitQueryParam,
            cursorQueryParam,
            useDraftsQueryParam,
            filters,
        );

        // Assert
        expect(firstPageOfLevels).to.deep.equal({
            levels: [
                {
                    "id": "2-2-2-2-2",
                    "name": "Level 2",
                    "creator": {
                        "id": "user-1-id",
                        "name": "User 1",
                    },
                    "updatedAt": 2,
                    "data": {},
                    "status": "PUBLISHED",
                    "createdAt": 1698514557729,
                    "avgScore": 1.0,
                    "totalScores": 2,
                    "avgRating": 1,
                    "totalRatings": 2,
                    "labels": [ "test" ],
                }
            ],
            cursor: "2-2-2-2-2",
        });
    })
});