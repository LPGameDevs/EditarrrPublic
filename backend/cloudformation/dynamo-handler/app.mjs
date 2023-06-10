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

const client = new DynamoDBClient({});

const dynamo = DynamoDBDocumentClient.from(client);

const tableName = "levels-table";

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
    switch (event.routeKey) {
      case "POST /levels":
        requestJSON = JSON.parse(event.body);
        let generatedLevelId = uuidv4();
        await dynamo.send(
          new PutCommand({
            TableName: tableName,
            Item: {
              id: generatedLevelId,
              lastUpdated: Date.now().toString(),
              name: requestJSON.name,
              creator: requestJSON.username,
              status: requestJSON.status,
              levelData: requestJSON.levelData,
            },
          })
        );
        body = `Created new level ${generatedLevelId}`;
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
        break;
      case "GET /levels":
        body = await dynamo.send(
          new QueryCommand({ 
            TableName: tableName,
            IndexName: "StatusLastUpdatedIndex",
            Limit: 10,
            KeyConditionExpression: "#status = :status",
            ExpressionAttributeNames: {
              "#status": "status"
            },
            ExpressionAttributeValues: {
              ":status": "published"
            },
            ScanIndexForward: false,
          })
        );
        break;
      case "PUT /levels/{id}":
        requestJSON = JSON.parse(event.body);
        await dynamo.send(
          new PutCommand({
            TableName: tableName,
            Item: {
              id: event.pathParameters.id,
              lastUpdated: Date.now().toString(),
              name: requestJSON.name,
              status: requestJSON.status,
              levelData: requestJSON.levelData,
            },
          })
        );
        body = `Updated level ${event.pathParameters.id}`;
        break;
      case "DELETE /levels/{id}":
        await dynamo.send(
          new DeleteCommand({
            TableName: tableName,
            Key: {
              id: event.pathParameters.id,
            },
          })
        );
        body = `Deleted level ${event.pathParameters.id}`;
        break;
      default:
        throw new Error(`Unsupported route: "${event.routeKey}"`);
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