import { DynamoDBClient } from "@aws-sdk/client-dynamodb";
import {
    DynamoDBDocumentClient,
    QueryCommand,
    PutCommand,
    GetCommand,
} from "@aws-sdk/lib-dynamodb";
import crypto from "crypto";
/* TODO:
    * Refactor - Move all the code for each API into its own file
    * Unit Tests
*/

let options = {};
if(process.env.AWS_SAM_LOCAL) {
    console.log("Setting IP Address of local DynamoDB Container to: %s", process.env.DDB_IP_ADDR);
    options.endpoint = "http://" + process.env.DDB_IP_ADDR + ":8000";
}

const client = new DynamoDBClient(options);

const dynamo = DynamoDBDocumentClient.from(client);

const tableName = "editarrr-level-storage";

// From https://stackoverflow.com/questions/105034/how-do-i-create-a-guid-uuid - not sure if this is reliable haha
function uuidv4() {
    return ([1e7]+-1e3+-4e3+-8e3+-1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

class BadRequestException extends Error {
    constructor(message) {
        super(`error - Bad Request: ${message}`);
        this.name = this.constructor.name;
    }
}

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

                var generatedLevelId = uuidv4();
                var currentTimestamp = Date.now();

                await dynamo.send(
                    new PutCommand({
                        TableName: tableName,
                        Item: {
                            pk: `LEVEL#${generatedLevelId}`,
                            sk: `LEVEL#${generatedLevelId}`,
                            levelName: levelName,
                            levelCreatorId: levelCreatorId,
                            levelCreatorName: levelCreatorName,
                            levelStatus: levelStatus,
                            levelCreatedAt: currentTimestamp,
                            levelUpdatedAt: currentTimestamp,
                            levelData: levelData
                        },
                    })
                );

                responseBody = {
                    "message": `Success! Created level: ${levelName}`
                }
                break;
            case "GET /levels":
                // TODO Validation of request

                // TODO Inclusion of request params in the query

                var queryResponse = await dynamo.send(
                    new QueryCommand({
                        TableName: tableName,
                        IndexName: "levelStatus-levelUpdatedAt-index",
                        Select: "ALL_PROJECTED_ATTRIBUTES",
                        Limit: 10,
                        ScanIndexForward: false,
                        KeyConditionExpression: "levelStatus = :status",
                        ExpressionAttributeValues: {
                            ":status": "PUBLISHED"
                        }
                    })
                );

                // TODO Validation of response

                var dbLevels = queryResponse.Items;

                var responseLevels = [];
                for (let i = 0; i < dbLevels.length; i++) {
                    var dbLevel = dbLevels[i];

                    // TODO Validation

                    var id = extractLevelId(dbLevel.pk);

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
                    });
                }

                responseBody = {
                    "levels": responseLevels
                }

                break;
            case "GET /levels/{id}":
                // TODO Validation of request

                var queryResponse = await dynamo.send(
                    new GetCommand({
                        TableName: tableName,
                        Key: {
                            "pk": `LEVEL#${event.pathParameters.id}`,
                            "sk": `LEVEL#${event.pathParameters.id}`
                        }
                    })
                );

                if (!queryResponse.Item) throw new Error(`Level ${event.pathParameters.id} not found`);

                var dbLevel = queryResponse.Item;

                // TODO Validation of queried response

                // TODO Refactor into a separate function
                var id = extractLevelId(dbLevel.pk);

                responseBody = {
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

                break;
            case "PATCH /levels/{id}":
                requestJSON = JSON.parse(event.body);
                var updatedLevelName = requestJSON.name;
                var updatedStatus = requestJSON.status;
                var updatedData = requestJSON.data;

                // TODO Validation of request

                var queryResponse = await dynamo.send(
                    new GetCommand({
                        TableName: tableName,
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

                await dynamo.send(
                    new PutCommand({
                        TableName: tableName,
                        Item: {
                            pk: dbLevel.pk,
                            sk: dbLevel.sk,
                            levelName: dbLevel.levelName,
                            levelCreatorId: dbLevel.levelCreatorId,
                            levelStatus: dbLevel.levelStatus,
                            levelCreatedAt: dbLevel.levelCreatedAt,
                            levelUpdatedAt: dbLevel.levelUpdatedAt,
                            levelData: dbLevel.levelData
                        },
                    })
                );

                var currentLevelName = dbLevel.levelName;
                responseBody = {
                    "message": `Success! Update level: ${currentLevelName}`
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

function extractLevelId(ddbLevelKeyStr) {
    const splitDDBLevelKeyStr = ddbLevelKeyStr.match(/#([0-9a-f-]+)/i);

    if (!splitDDBLevelKeyStr) {
        throw new Error(`problem parsing database ID`)
    }

    return splitDDBLevelKeyStr[1];
}
