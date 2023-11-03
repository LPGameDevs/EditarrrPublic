import {
    PutCommand, QueryCommand
} from "@aws-sdk/lib-dynamodb";

import { uuidv4 } from "../utils.mjs";

const tableName = "editarrr-rating-storage";
const indexLevelIdRating = "pk-rating-index"

export class RatingsDbClient {
    constructor(dynamoDbClient) {
        this.dynamoDbClient = dynamoDbClient;
    }

    // TODO Move other ratings queries here

    async putRating(
        rating, 
        levelId, 
        ratingLevelName, 
        ratingCreatorId, 
        ratingCreatorName,
    ) {
        var generatedRatingId = uuidv4();
        var currentTimestamp = Date.now();

        await this.dynamoDbClient.send(
            new PutCommand({
                TableName: tableName,
                Item: {
                    pk: `LEVEL#${levelId}`,
                    sk: `SCORE#${generatedRatingId}`,
                    rating: rating,
                    ratingLevelName: ratingLevelName,
                    ratingCreatorId: ratingCreatorId,
                    ratingCreatorName: ratingCreatorName,
                    ratingSubmittedAt: currentTimestamp,
                },
            })
        );

        return generatedRatingId;
    }

    async getAllRatingsForLevel(levelId) {
        var queryResponse = await this.dynamoDbClient.send(
            new QueryCommand({
                TableName: tableName,
                IndexName: indexLevelIdRating,
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