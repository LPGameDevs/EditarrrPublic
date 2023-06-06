import {DynamoDBClient} from "@aws-sdk/client-dynamodb";
import {
    DynamoDBDocumentClient,
    ScanCommand,
    PutCommand,
    GetCommand,
    DeleteCommand,
} from "@aws-sdk/lib-dynamodb";

const client = new DynamoDBClient({});

const dynamo = DynamoDBDocumentClient.from(client);

const tableName = "http-crud-tutorial-items";

export const handler = async (event, context) => {
    let body;
    let statusCode = 200;
    const headers = {
        "Content-Type": "application/json",
    };

    try {
        switch (event.requestContext.resourceId) {
            case "DELETE /items/{id}":
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
            case "GET /items/{id}":
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
            case "GET /items":
                body = await dynamo.send(
                    new ScanCommand({TableName: tableName})
                );
                body = body.Items;
                break;
            case "PUT /items":
                let requestJSON = JSON.parse(event.body);
                await dynamo.send(
                    new PutCommand({
                        TableName: tableName,
                        Item: {
                            id: requestJSON.id,
                            price: requestJSON.price,
                            name: requestJSON.name,
                        },
                    })
                );
                body = `Put item ${requestJSON.id}`;
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
        body: JSON.stringify({
            data: body,
            // debug: event,
        }),
        headers,
    };
};


// export const handler = async (event, context) => {
//     console.log('Event: ', event);
//     let responseMessage = 'Hello, World!!!';
//
//     if (event.queryStringParameters && event.queryStringParameters['Name']) {
//         responseMessage = 'Hello, ' + event.queryStringParameters['Name'] + '!';
//     }
//
//     if (event.httpMethod === 'POST') {
//         const body = JSON.parse(event.body);
//         responseMessage = 'Hello, ' + body.name + '!';
//     }
//
//     const response = {
//         statusCode: 200,
//         headers: {
//             'Content-Type': 'application/json',
//         },
//         body: JSON.stringify({
//             message: responseMessage,
//             other: event,
//         }),
//     };
//
//     return response;
// };
