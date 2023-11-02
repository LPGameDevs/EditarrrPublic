import {
    PutCommand, QueryCommand
} from "@aws-sdk/lib-dynamodb";

import { uuidv4 } from "../utils.mjs";

const tableName = "editarrr-score-storage";

export class ScoresDbClient {
    constructor(dynamoDbClient) {
        this.dynamoDbClient = dynamoDbClient;
    }

    // TODO Move other scores queries here

    async putScore(
        score, 
        levelId, 
        scoreLevelName, 
        scoreCreatorId, 
        scoreCreatorName,
    ) {
        var generatedScoreId = uuidv4();
        var currentTimestamp = Date.now();

        await this.dynamoDbClient.send(
            new PutCommand({
                TableName: tableName,
                Item: {
                    pk: `LEVEL#${levelId}`,
                    sk: `SCORE#${generatedScoreId}`,
                    score: score,
                    scoreLevelName: scoreLevelName,
                    scoreCreatorId: scoreCreatorId,
                    scoreCreatorName: scoreCreatorName,
                    scoreSubmittedAt: currentTimestamp,
                },
            })
        );

        return generatedScoreId;
    }

    async getAllScoresForLevel(levelId) {
        var queryResponse = await this.dynamoDbClient.send(
            new QueryCommand({
                TableName: tableName,
                IndexName: "scoreLevelName-score-index",
                // TODO can we do a subset of attributes?
                Select: "ALL_PROJECTED_ATTRIBUTES",
                KeyConditionExpression: "pk = :levelId",
                ExpressionAttributeValues: {
                    ":levelId": "LEVEL#" + levelId
                }
        }));

        // TODO Validation of response

        return queryResponse.Items;
    }
}