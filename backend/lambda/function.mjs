import { Buffer } from 'buffer';

import {DynamoDBClient} from "@aws-sdk/client-dynamodb";
import {
    DynamoDBDocumentClient,
    ScanCommand,
    QueryCommand,
    PutCommand,
    GetCommand,
} from "@aws-sdk/lib-dynamodb";
import {
    S3Client,
    PutObjectCommand
} from "@aws-sdk/client-s3";

import { BadRequestException, extractId, uuidv4 } from "./utils.mjs";
import { LevelsDbClient, LevelsSortOptions } from './db/levels.mjs';
import { LevelsApi } from "./api/levels.mjs";
import { ScoresDbClient } from './db/scores.mjs';
import { ScoresApi } from './api/scores.mjs';
import { RatingsDbClient } from './db/ratings.mjs';
import { RatingsApi } from './api/ratings.mjs';

/* TODO:
    * Refactor - Move all the code for each API into its own file
    * Unit Tests
*/

let options = {};
if (process.env.AWS_SAM_LOCAL) {
    console.log("Setting IP Address of local DynamoDB Container to: %s", process.env.DDB_IP_ADDR);
    options.endpoint = "http://" + process.env.DDB_IP_ADDR + ":8000";
}

const client = new DynamoDBClient(options);

const dynamo = DynamoDBDocumentClient.from(client);

const screenshotBucketProd = "editarrr-screenshot-ethical-hare";
const screenshotBucketDev = "editarrr-screenshot-ideal-wren";
const tableNameLevel = "editarrr-level-storage";
// TODO Move all calls to the level storage to this client
const levelsDbClient = new LevelsDbClient(dynamo);
// TODO Move levels API logic to this class
const levelsApi = new LevelsApi(levelsDbClient);

const tableNameScore = "editarrr-score-storage";
// TODO Move all calls to the level storage to this client
const scoresDbClient = new ScoresDbClient(dynamo);
// TODO Move levels API logic to this class
const scoresApi = new ScoresApi(scoresDbClient, levelsDbClient);

const tableNameRating = "editarrr-rating-storage";
// TODO Move all calls to the level storage to this client
const ratingsDbClient = new RatingsDbClient(dynamo);
// TODO Move levels API logic to this class
const ratingsApi = new RatingsApi(ratingsDbClient, levelsDbClient);

const tableNameAnalytics = "editarrr-analytics-storage";


export const handler = async (event, context) => {
    let requestJSON;
    let responseBody;
    let statusCode = 200;
    const headers = {
        "Content-Type": "application/json",
    };

    try {

        // TODO For some reason, deploys are only working 'event.requestContext.resourceId',
        // but local development only works with 'event.requestContext.routeKey'.
        // Logging to find out what information is in the event so we can determine the appropriate fix.
        console.log(`Lambda received event: ${JSON.stringify(event)}`);

        switch (event.requestContext.resourceId) {
            case "POST /levels":
                requestJSON = JSON.parse(event.body);

                var levelName = requestJSON.name;
                if (!levelName) throw new BadRequestException(`'name' must be provided in the request.`);
                var levelCreator = requestJSON.creator;
                if (!levelCreator) throw new BadRequestException(`'creator' must be provided in the request.`);
                var levelCreatorId = levelCreator.id;
                if (!levelCreatorId) throw new BadRequestException(`'creator.id' must be provided in the request.`);
                // TODO Validation of Creator (check that the ID exists)
                var levelCreatorName = levelCreator.name;
                if (!levelCreatorName) throw new BadRequestException(`'creator.name' must be provided in the request.`);
                // TODO Or should the creator name be based on stored User data?
                var levelStatus = requestJSON.status;
                if (!levelStatus) throw new BadRequestException(`'status' must be provided in the request.`);
                // TODO Further validation of status (introduce an enum?)
                var levelData = requestJSON.data;
                if (!levelData) throw new BadRequestException(`'data' must be provided in the request.`);
                var labels = requestJSON.labels ?? [];
                var version = requestJSON.version ?? 0;

                var generatedLevelId = uuidv4();
                var currentTimestamp = Date.now();

                await dynamo.send(
                    new PutCommand({
                        TableName: tableNameLevel,
                        Item: {
                            pk: `LEVEL#${generatedLevelId}`,
                            sk: `LEVEL#${generatedLevelId}`,
                            levelName: levelName,
                            levelCreatorId: levelCreatorId,
                            levelCreatorName: levelCreatorName,
                            levelStatus: levelStatus,
                            levelCreatedAt: currentTimestamp,
                            levelUpdatedAt: currentTimestamp,
                            version: version,
                            labels: labels,
                            levelData: levelData
                        },
                    })
                );

                responseBody = {
                    "message": `Success! Created level: ${levelName}`,
                    "id": generatedLevelId
                }
                break;
            case "GET /levels":
                var filters = {
                    anyOfLabels: event?.queryStringParameters?.["any-of-labels"]?.split(","),
                    nameContains: event?.queryStringParameters?.["nameContains"],
                };
                responseBody = await levelsApi.getPagedLevels(
                    event?.queryStringParameters?.["sort-option"],
                    event?.queryStringParameters?.["sort-asc"],
                    event?.queryStringParameters?.limit,
                    event.queryStringParameters?.cursor,
                    event?.queryStringParameters?.draft,
                    filters);
                break;
            case "GET /levels/{id}":
                // TODO Validation of request

                responseBody = await levelsApi.getLevel(event.pathParameters.id);

                break;
            case "PATCH /levels/{id}":
                requestJSON = JSON.parse(event.body);
                var updatedLevelName = requestJSON.name;
                var updatedStatus = requestJSON.status;
                var updatedData = requestJSON.data;
                var labels = requestJSON.labels ?? [];
                var version = requestJSON.version ?? 0;

                // TODO Validation of request

                // TODO Replace this with the dbClient command
                var queryResponse = await dynamo.send(
                    new GetCommand({
                        TableName: tableNameLevel,
                        Key: {
                            pk: `LEVEL#${event.pathParameters.id}`,
                            sk: `LEVEL#${event.pathParameters.id}`
                        }
                    })
                );

                if (!queryResponse.Item) throw new Error(`Level ${event.pathParameters.id} not found`);

                var dbLevel = queryResponse.Item;

                // TODO Validation

                if (updatedLevelName) {
                    dbLevel.levelName = updatedLevelName;
                }
                if (updatedStatus) {
                    dbLevel.levelStatus = updatedStatus;
                }
                if (updatedData) {
                    dbLevel.levelData = updatedData;
                }
                dbLevel.levelUpdatedAt = Date.now();

                // TODO We should make this an "update" rather than a "put" (https://docs.aws.amazon.com/cli/latest/reference/dynamodb/update-item.html)
                // because it eliminates the need for a "get"
                await dynamo.send(
                    new PutCommand({
                        TableName: tableNameLevel,
                        Item: {
                            pk: dbLevel.pk,
                            sk: dbLevel.sk,
                            levelName: dbLevel.levelName,
                            levelCreatorId: dbLevel.levelCreatorId,
                            levelCreatorName: dbLevel.levelCreatorName,
                            levelStatus: dbLevel.levelStatus,
                            levelCreatedAt: dbLevel.levelCreatedAt,
                            levelUpdatedAt: dbLevel.levelUpdatedAt,
                            levelData: dbLevel.levelData,
                            version: version,
                            labels: labels,
                        },
                    })
                );

                var currentLevelName = dbLevel.levelName;
                responseBody = {
                    "message": `Success! Update level: ${currentLevelName}`
                }
                break;

            case "POST /screenshot/{filename}":
                const s3Client = new S3Client({});
                const imageBuffer = Buffer.from(event.body, 'base64');

                // Set the parameters
                const params = {
                    Bucket: screenshotBucketDev,
                    Key: event.pathParameters.filename,
                    Body: imageBuffer,
                    ACL: "public-read",
                };

                const results = await s3Client.send(new PutObjectCommand(params));

                const message = "Successfully created " +
                    params.Key +
                    " and uploaded it to " +
                    params.Bucket +
                    "/" +
                    params.Key;
                responseBody = {
                    "message": message
                }
                break;

            case "POST /levels/{id}/scores":
                requestJSON = JSON.parse(event.body);
                responseBody = await scoresApi.postScore(event.pathParameters.id, requestJSON);
                break;
            case "GET /levels/{id}/scores":
                responseBody = await scoresApi.getPagedScores(
                    event.pathParameters.id,
                    event?.queryStringParameters?.["sort-option"],
                    event?.queryStringParameters?.["sort-asc"],
                    event?.queryStringParameters?.limit,
                    event?.queryStringParameters?.cursor,
                    event?.queryStringParameters?.["de-dupe-by-user"]);
                break;

            case "POST /levels/{id}/ratings":
                requestJSON = JSON.parse(event.body);
                responseBody = await ratingsApi.postRating(event.pathParameters.id, requestJSON);
                break;
            case "GET /levels/{id}/ratings":
                // TODO Validation of request

                // TODO Inclusion of request params in the query

                var queryResponse = await dynamo.send(
                    new QueryCommand({
                        TableName: tableNameRating,
                        IndexName: "pk-rating-index",
                        Select: "ALL_PROJECTED_ATTRIBUTES",
                        Limit: 10,
                        ScanIndexForward: false,
                        KeyConditionExpression: "pk = :levelId",
                        ExpressionAttributeValues: {
                            ":levelId": "LEVEL#" + event.pathParameters.id
                        }
                    })
                );

                // TODO Validation of response

                var dbRatings = queryResponse.Items;

                var responseRatings = [];
                for (let i = 0; i < dbRatings.length; i++) {
                    var dbRating = dbRatings[i];

                    // TODO Validation

                    var id = extractId(dbRating.sk);
                    var levelId = extractId(dbRating.pk);

                    responseRatings.push({
                        "ratingId": id,
                        "levelId": levelId,
                        "rating": dbRating.rating,
                        "code": dbRating.ratingLevelName,
                        "creator": {
                            "id": dbRating.ratingCreatorId,
                            "name": dbRating.ratingCreatorName
                        },
                        "submittedAt": dbRating.ratingSubmittedAt,
                    });
                }

                responseBody = {
                    "ratings": responseRatings
                }
                break;

            case "POST /analytics":
                requestJSON = JSON.parse(event.body);

                var type = requestJSON.type;
                if (!type) throw new BadRequestException(`'type' must be provided in the request.`);
                var value = requestJSON.value;
                if (!value) throw new BadRequestException(`'value' must be provided in the request.`);
                var creatorId = requestJSON.creatorId;
                if (!creatorId) throw new BadRequestException(`'creatorId' must be provided in the request.`);

                var creatorName = requestJSON.creatorName;
                if (!creatorName) throw new BadRequestException(`'creatorName' must be provided in the request.`);

                var generatedAnalyticsId = uuidv4();
                var currentTimestamp = Date.now();

                await dynamo.send(
                    new PutCommand({
                        TableName: tableNameAnalytics,
                        Item: {
                            pk: `USER#${creatorId}`,
                            sk: `ANALYTICS#${generatedAnalyticsId}`,
                            type: type,
                            value: value,
                            creatorName: creatorName,
                            analyticsSubmittedAt: currentTimestamp,
                        },
                    })
                );

                responseBody = {
                    "message": `Success! Created analytics for: ${creatorName}`,
                    "id": generatedAnalyticsId
                }
                break;
            case "GET /user/{id}/analytics":
                // TODO Validation of request

                // TODO Inclusion of request params in the query

                var queryResponse = await dynamo.send(
                    new QueryCommand({
                        TableName: tableNameAnalytics,
                        IndexName: "pk-sk-index",
                        Select: "ALL_PROJECTED_ATTRIBUTES",
                        Limit: 10,
                        ScanIndexForward: false,
                        KeyConditionExpression: "pk = :userId",
                        ExpressionAttributeValues: {
                            ":userId": "USER#" + event.pathParameters.id
                        }
                    })
                );

                // TODO Validation of response

                var dbAnalytics = queryResponse.Items;

                var responseAnalytics = [];
                for (let i = 0; i < dbAnalytics.length; i++) {
                    var dbAnalytic = dbAnalytics[i];

                    // TODO Validation

                    var id = extractId(dbAnalytic.sk);
                    var userId = extractId(dbAnalytic.pk);

                    responseAnalytics.push({
                        "analyticsId": id,
                        "type": dbAnalytic.type,
                        "value": dbAnalytic.value,
                        "creator": {
                            "id": userId,
                            "name": dbAnalytic.creatorName
                        },
                        "submittedAt": dbAnalytic.analyticsSubmittedAt,
                    });
                }

                responseBody = {
                    "analytics": responseAnalytics
                }
                break;
            case "GET /analytics/{type}":
                // TODO Validation of request

                // TODO Inclusion of request params in the query

                var queryResponse = await dynamo.send(
                    new QueryCommand({
                        TableName: tableNameAnalytics,
                        IndexName: "type-sk-index",
                        Select: "ALL_PROJECTED_ATTRIBUTES",
                        Limit: 10,
                        ScanIndexForward: false,
                        KeyConditionExpression: "type = :type",
                        ExpressionAttributeValues: {
                            ":type": event.pathParameters.type
                        }
                    })
                );

                // TODO Validation of response

                var dbAnalytics = queryResponse.Items;

                var responseAnalytics = [];
                for (let i = 0; i < dbAnalytics.length; i++) {
                    var dbAnalytic = dbAnalytics[i];

                    // TODO Validation

                    var id = extractId(dbAnalytic.sk);
                    var userId = extractId(dbAnalytic.pk);

                    responseAnalytics.push({
                        "analyticsId": id,
                        "type": dbAnalytic.type,
                        "value": dbAnalytic.value,
                        "creator": {
                            "id": userId,
                            "name": dbAnalytic.creatorName
                        },
                        "submittedAt": dbAnalytic.analyticsSubmittedAt,
                    });
                }

                responseBody = {
                    "analytics": responseAnalytics
                }
                break;

            case "GET /all/levels":
                // TODO Validation of request

                // TODO Inclusion of request params in the query

                var scanResponse = await dynamo.send(
                    new ScanCommand({
                        TableName: tableNameLevel,
                        // ExpressionAttributeNames: {
                        // "#AT": "AlbumTitle",
                        // },
                        // ProjectionExpression: "#ST, #AT",
                    })
                );

                // TODO Validation of response

                var dbLevels = scanResponse.Items;

                var responseLevels = [];
                for (let i = 0; i < dbLevels.length; i++) {
                    var dbLevel = dbLevels[i];

                    // TODO Validation

                    var id = extractId(dbLevel.pk);

                    responseLevels.push({
                        "id": id,
                        "name": dbLevel.levelName,
                        "creator": {
                            "id": dbLevel.levelCreatorId,
                            "name": dbLevel.levelCreatorName
                        },
                        "status": dbLevel.levelStatus,
                        "updatedAt": dbLevel.levelUpdatedAt,
                        "createdAt": dbLevel.levelCreatedAt,
                        "version": dbLevel.version ?? 0,
                        "labels": dbLevel.labels ?? [],
                        "levelTotalRatings": dbLevel.levelTotalRatings ?? 0,
                        "levelTotalScores": dbLevel.levelTotalScores ?? 0,
                        "levelAvgRating": dbLevel.levelAvgRating ?? 0,
                        "levelAvgScore": dbLevel.levelAvgScore ?? 0,
                    });
                }

                responseBody = {
                    "levels": responseLevels
                }
                break;
            case "GET /all/scores":
                // TODO Validation of request

                // TODO Inclusion of request params in the query

                var scanResponse = await dynamo.send(
                    new ScanCommand({
                        TableName: tableNameScore,
                        // ExpressionAttributeNames: {
                        // "#AT": "AlbumTitle",
                        // },
                        // ProjectionExpression: "#ST, #AT",
                    })
                );

                // TODO Validation of response

                var dbScores = scanResponse.Items;

                var responseScores = [];
                for (let i = 0; i < dbScores.length; i++) {
                    var dbScore = dbScores[i];

                    // TODO Validation

                    var id = extractId(dbScore.sk);
                    var levelId = extractId(dbScore.pk);

                    responseScores.push({
                        "scoreId": id,
                        "levelId": levelId,
                        "scoreNumber": dbScore.scoreNumber,
                        "code": dbScore.scoreLevelName,
                        "creator": {
                            "id": dbScore.scoreCreatorId,
                            "name": dbScore.scoreCreatorName
                        },
                        "submittedAt": dbScore.scoreSubmittedAt,
                    });
                }

                responseBody = {
                    "scores": responseScores
                }
                break;
            case "GET /all/ratings":
                // TODO Validation of request

                // TODO Inclusion of request params in the query

                var scanResponse = await dynamo.send(
                    new ScanCommand({
                        TableName: tableNameRating,
                        // ExpressionAttributeNames: {
                        // "#AT": "AlbumTitle",
                        // },
                        // ProjectionExpression: "#ST, #AT",
                    })
                );

                // TODO Validation of response

                var dbRatings = scanResponse.Items;


                var responseRatings = [];
                for (let i = 0; i < dbRatings.length; i++) {
                    var dbRating = dbRatings[i];

                    // TODO Validation

                    var id = extractId(dbRating.sk);
                    var levelId = extractId(dbRating.pk);

                    responseRatings.push({
                        "ratingId": id,
                        "levelId": levelId,
                        "rating": dbRating.rating,
                        "code": dbRating.ratingLevelName,
                        "creator": {
                            "id": dbRating.ratingCreatorId,
                            "name": dbRating.ratingCreatorName
                        },
                        "submittedAt": dbRating.ratingSubmittedAt,
                    });
                }

                responseBody = {
                    "ratings": responseRatings
                }
                break;

            case "GET /all/analytics":
                // TODO Validation of request

                // TODO Inclusion of request params in the query

                var scanResponse = await dynamo.send(
                    new ScanCommand({
                        TableName: tableNameAnalytics,
                        // ExpressionAttributeNames: {
                        // "#AT": "AlbumTitle",
                        // },
                        // ProjectionExpression: "#ST, #AT",
                    })
                );

                // TODO Validation of response

                var dbAnalytics = scanResponse.Items;

                var responseAnalytics = [];
                var loopLimit = Math.min(1000, dbAnalytics.length); // Limit the loop to 100 items or the array length, whichever is smaller

                for (let i = 0; i < loopLimit; i++) {
                    var dbAnalytic = dbAnalytics[i];

                    // TODO Validation

                    var id = extractId(dbAnalytic.sk);
                    var creatorId = extractId(dbAnalytic.pk);

                    responseAnalytics.push({
                        "analyticId": id,
                        "type": dbAnalytic.type,
                        "value": dbAnalytic.value ?? "",
                        "creator": {
                            "id": creatorId ?? "",
                            "name": dbAnalytic.creatorName ?? ""
                        },
                        "submittedAt": dbAnalytic.analyticsSubmittedAt ?? 0
                    });
                }

                responseBody = {
                    "analytics": responseAnalytics,
                    "total": dbAnalytics.length
                }
                break;

            default:
                throw new Error(`Unsupported route: "${event.requestContext.resourceId}"`);
        }
    } catch (err) {
        if (err instanceof BadRequestException) {
            statusCode = 400;
            // TODO 404 errors
        } else {
            statusCode = 500;
        }
        responseBody = err.message;
    } finally {
        responseBody = JSON.stringify(responseBody);
    }

    return {
        statusCode,
        body: responseBody,
        headers,
    };
};
