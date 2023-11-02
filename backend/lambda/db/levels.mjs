import {
    GetCommand, UpdateCommand,
} from "@aws-sdk/lib-dynamodb";

const tableNameLevel = "editarrr-level-storage";

export class LevelsDbClient {
    constructor(dynamoDbClient) {
        this.dynamoDbClient = dynamoDbClient;
    }

    // TODO Move other level queries here

    async getLevel(levelId) {
        var getResponse = await this.dynamoDbClient.send(
            new GetCommand({
                TableName: tableNameLevel,
                Key: {
                    pk: `LEVEL#${levelId}`,
                    sk: `LEVEL#${levelId}`
                }
            })
        );

        if (!getResponse?.Item) throw new Error(`Level ${levelId} not found`);

        return getResponse.Item;
    }

    async updateLevelScoreData(levelId, avgScore, totalNumberOfScores) {
        await this.dynamoDbClient.send(
            // https://docs.aws.amazon.com/AWSJavaScriptSDK/v3/latest/classes/_aws_sdk_lib_dynamodb.UpdateCommand.html
            // https://docs.aws.amazon.com/AWSJavaScriptSDK/v3/latest/client/dynamodb/command/UpdateItemCommand/
            // https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/GettingStarted.UpdateItem.html
            new UpdateCommand({
                TableName: tableNameLevel,
                Key: {
                    pk: `LEVEL#${levelId}`,
                    sk: `LEVEL#${levelId}`
                },
                UpdateExpression: "SET levelAvgScore = :avgScore, levelTotalScores = :totalScores",
                ExpressionAttributeValues: {
                  ":avgScore": avgScore,
                  ":totalScores": totalNumberOfScores,
                },
            })
        );
    }
}