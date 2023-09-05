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
            case "GET /levels":
                body = await dynamo.send(
                    // TODO Actual Implementation
                    new ScanCommand({
                        TableName: tableName
                    })
                );
                break;
            case "GET /levels/{id}":
                body = await dynamo.send(
                    new GetCommand({
                        TableName: tableName,
                        Key: {
                            id: event.pathParameters.id,
                        },
                    })
                );
                if (!body.Item) throw new Error(`Item ${event.pathParameters.id} not found`);
                body = body.Item;
                break;
            case "POST /levels":
                const requestData = JSON.parse(event.body);
                let generatedLevelId = uuidv4();
                await dynamo.send(
                    // TODO Actual implementation, this was just inserting some dummy data for testing
                    new PutCommand({
                        TableName: tableName,
                        Item: {
                            pk: "USER#user1",
                            sk: "USER#user1",
                            userName: "User 1"
                        },
                    })
                );
                body = `Created new level ${generatedLevelId}`;
                break;
            case "PUT /levels/{id}":
                let requestJSON = JSON.parse(event.body);
                await dynamo.send(
                    new PutCommand({
                        TableName: tableName,
                        Item: {
                            id: event.pathParameters.id,
                            lastUpdated: Date.now().toString(),
                            name: requestJSON.name,
                            status: requestJSON.status,
                            creator: requestData.username,
                            levelData: requestJSON.levelData,
                        },
                    })
                );
                body = `Updated level ${event.pathParameters.id}`;
                break;
            case "DELETE /levels/{id}":
                // We dont actually want to delete so lets just unpublish.

                // await dynamo.send(
                //     new DeleteCommand({
                //         TableName: tableName,
                //         Key: {
                //             id: event.pathParameters.id,
                //         },
                //     })
                // );

                let item = await dynamo.send(
                    new GetCommand({
                        TableName: tableName,
                        Key: {
                            id: event.pathParameters.id,
                        },
                    })
                );

                if (!item.Item) throw new Error(`Item ${event.pathParameters.id} not found`);

                item = item.Item;
                item.published = false;

                await dynamo.send(
                    new PutCommand({
                        TableName: tableName,
                        Item: item,
                    })
                );

                body = `Deleted item ${event.pathParameters.id}`;
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
