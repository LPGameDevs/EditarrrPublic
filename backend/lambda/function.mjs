import { DynamoDBClient } from "@aws-sdk/client-dynamodb";
import {
    DynamoDBDocumentClient,
    ScanCommand,
    QueryCommand,
    PutCommand,
    GetCommand,
    DeleteCommand,
} from "@aws-sdk/lib-dynamodb";
import crypto from "crypto";

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

export const handler = async (event, context) => {
    let body;
    let requestJSON;
    let statusCode = 200;
    const headers = {
        "Content-Type": "application/json",
    };

    try {
        switch (event.requestContext.routeKey) {
            case "POST /levels":
                // TODO Actual Implementation
                requestJSON = JSON.parse(event.body);
                var levelName = requestJSON.name; 
                body = {
                    "message": `Success! Created level: ${levelName}`
                }
                // const requestData = JSON.parse(event.body);
                // let generatedLevelId = uuidv4();
                // await dynamo.send(
                //     // TODO Actual implementation, this was just inserting some dummy data for testing
                //     new PutCommand({
                //         TableName: tableName,
                //         Item: {
                //             pk: "USER#user1",
                //             sk: "USER#user1",
                //             userName: "User 1"
                //         },
                //     })
                // );
                // body = `Created new level ${generatedLevelId}`;
                break;
            case "GET /levels":
                // TODO Actual Implementation
                body = {
                    "levels": [
                        {
                            "id": "level2",
                            "name": "Level 2",
                            "creator": {
                                "id": "user2",
                                "name": "User 2"
                            },
                            "status": "published",
                            "createdAt": 1695649746,
                            "updatedAt": 1695649746,
                        },
                        {
                            "id": "level1",
                            "name": "Level 1",
                            "creator": {
                                "id": "user1",
                                "name": "User 1"
                            },
                            "status": "published",
                            "createdAt": 1686495335,
                            "updatedAt": 1686495335,
                        }
                    ]
                  }
                // body = await dynamo.send(
                //     // TODO Actual Implementation
                //     new ScanCommand({
                //         TableName: tableName
                //     })
                // );
                break;
            case "GET /levels/{id}":
                // TODO Actual Implementation
                var levelId = event.pathParameters.id;
                body = {
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
                body = {
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
        statusCode = 400;
        body = err.message;
    } finally {
        body = JSON.stringify(body);
    }

    return {
        statusCode,
        body,
        headers,
    };
};
