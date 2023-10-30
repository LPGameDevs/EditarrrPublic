import {
    GetCommand,
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
}