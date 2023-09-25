import { DynamoDBClient } from "@aws-sdk/client-dynamodb";
import {
    DynamoDBDocumentClient,
    QueryCommand,
    PutCommand,
    GetCommand,
} from "@aws-sdk/lib-dynamodb";
import crypto from "crypto";
import { request } from "http";

/* TODO Refactor Ideas
 * Separate files for different APIs?
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
        switch (event.requestContext.routeKey) {
            case "POST /levels":
                requestJSON = JSON.parse(event.body);
                var levelName = requestJSON.name;
                if (!levelName) throw new BadRequestException(`'name' must be provided in the request.`);
                var levelCreator = requestJSON.creator;
                if (!levelCreator) throw new BadRequestException(`'creator' must be provided in the request.`);
                var levelCreatorId = levelCreator.id;
                if (!levelCreatorId) throw new BadRequestException(`'creator.id' must be provided in the request.`);
                var levelCreatorName = levelCreator.name;
                if (!levelCreatorName) throw new BadRequestException(`'creator.name' must be provided in the request.`);
                // TODO Further validation of creator - maybe checking that the ID even exists?
                var levelStatus = requestJSON.status;
                if (!levelStatus) throw new BadRequestException(`'status' must be provided in the request.`);
                // TODO Further validation of status (ensuring it is a valid one)
                var levelData = requestJSON.data; 
                if (!levelData) throw new BadRequestException(`'data' must be provided in the request.`);
                
                var generatedLevelId = uuidv4();
                var currentTimestamp = Date.now();

                // TODO We aren't actually storing the levelCreatorName...should we? I think so

                await dynamo.send(
                    new PutCommand({
                        TableName: tableName,
                        Item: {
                            pk: `LEVEL#${generatedLevelId}`,
                            sk: `LEVEL#${generatedLevelId}`,
                            levelName: levelName,
                            levelCreatorId: levelCreatorId,
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
                    // Docs for QueryCommand:
                    // https://docs.aws.amazon.com/AWSJavaScriptSDK/v3/latest/clients/client-dynamodb/classes/querycommand.html
                    // https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Query.html
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
                // TODO Validation

                var queryResponseLevels = queryResponse.Items;
                // TODO Validation

                var responseLevels = []; 
                for (let i = 0; i < queryResponseLevels.length; i++) {
                    var queryResponseLevel = queryResponseLevels[i];

                    // TODO Validation

                    // Use a regular expression to match and extract the ID
                    const match = queryResponseLevel.pk.match(/#([0-9a-f-]+)/i);

                    var id;
                    if (match) {
                        id = match[1];
                        console.log("Extracted ID:", id);
                    } else {
                        console.log("ID not found in the string.");
                    }
                    
                    // TODO Bug: CreatedAt not being returned?

                    responseLevels.push({
                        "id": id,
                        "name": queryResponseLevel.levelName,
                        "creator": {
                            "id": queryResponseLevel.levelCreatorId,
                            "name": queryResponseLevel.levelCreatorName
                        },
                        "status": queryResponseLevel.levelStatus,
                        "createdAt": queryResponseLevel.levelCreatedAt,
                        "updatedAt": queryResponseLevel.levelUpdatedAt,
                    });
                }

                responseBody = {
                    "levels": responseLevels
                }

                break;
            case "GET /levels/{id}":
                // TODO Actual Implementation
                var levelId = event.pathParameters.id;
                responseBody = {
                    "id": levelId,
                    "name": `Level ${levelId}`,
                    "creator": {
                      "id": "user1",
                      "name": "User 1"
                    },
                    "status": "published",
                    "createdAt": 1686495335,
                    "updatedAt": 1686495335,
                    "data": {}
                }
                // body = await dynamo.send(
                //     new GetCommand({
                //         TableName: tableName,
                //         Key: {
                //             id: event.pathParameters.id,
                //         },
                //     })
                // );
                // if (!body.Item) throw new Error(`Item ${event.pathParameters.id} not found`);
                // body = body.Item;
                break;
            case "PUT /levels/{id}":
                // TODO Actual Implementation
                requestJSON = JSON.parse(event.body);
                var levelName = requestJSON.name; 
                responseBody = {
                    "message": `Success! Update level: ${levelName}`
                }
                // let requestJSON = JSON.parse(event.body);
                // await dynamo.send(
                //     new PutCommand({
                //         TableName: tableName,
                //         Item: {
                //             id: event.pathParameters.id,
                //             lastUpdated: Date.now().toString(),
                //             name: requestJSON.name,
                //             status: requestJSON.status,
                //             creator: requestData.username,
                //             levelData: requestJSON.levelData,
                //         },
                //     })
                // );
                // body = `Updated level ${event.pathParameters.id}`;
                break;
            default:
                throw new Error(`Unsupported route: "${event.requestContext.resourceId}"`);
        }
    } catch (err) {
        if (err instanceof BadRequestException) {
            statusCode = 400;
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
