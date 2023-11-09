import {
    GetCommand, PutCommand, QueryCommand
} from "@aws-sdk/lib-dynamodb";

import { uuidv4 } from "../utils.mjs";

const tableName = "editarrr-score-storage";

const indexScoreIdScore = "scoreLevelName-score-index"; // Note: I think the index was accidentally named this way - the index's pk is the main pk

// Because JS doesn't have enums...
export class ScoresSortOptions {
    static SCORE = "score";

    static isValid(str) {
        for (const validOption in ScoresSortOptions) {
            if (ScoresSortOptions[validOption] === str) {
                return true;
            }
        }
        return false;
    }

    static getAllValidValues() {
        return Object.values(ScoresSortOptions);
    }
}

const sortOptionToIndex = {
    [ScoresSortOptions.SCORE]: indexScoreIdScore,
}

const sortOptionToAttributeName = {
    [ScoresSortOptions.SCORE]: "score",
}

export class ScoresDbClient {
    constructor(dynamoDbClient) {
        this.dynamoDbClient = dynamoDbClient;
    }

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

    async getPagedScoresForLevel(
        levelId,
        sortOption,
        sortAsc,
        pageLimit, 
        cursor,
    ) {
        var query = {
            TableName: tableName,
            IndexName: sortOptionToIndex[sortOption],
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: pageLimit,
            ScanIndexForward: sortAsc, 
            KeyConditionExpression: "pk = :levelId",
            ExpressionAttributeValues: {
                ":levelId": "LEVEL#" + levelId
            }
        };

        if (cursor) {
            // Using scoreId as a cursor, we have to fetch more score data in order to provide DDB with all the data it expects from the cursor
            var cursorScoreData = await this.getScore(cursor);
            
            var sortedAttributeName = sortOptionToAttributeName[sortOption];

            query.ExclusiveStartKey = {
                [sortedAttributeName]: cursorScoreData[sortedAttributeName],
                pk: cursorScoreData.pk,
                sk: cursorScoreData.sk,
            };
        }

        var queryResponse = await this.dynamoDbClient.send(new QueryCommand(query));

        // TODO Validation of response

        return {
            scores: queryResponse.Items,
            cursor: queryResponse.LastEvaluatedKey,
        } 
    }

    async getAllScoresForLevel(levelId, sortAsc = true) {
        var queryResponse = await this.dynamoDbClient.send(
            new QueryCommand({
                TableName: tableName,
                IndexName: indexScoreIdScore,
                // TODO can we do a subset of attributes?
                Select: "ALL_PROJECTED_ATTRIBUTES",
                ScanIndexForward: sortAsc,
                KeyConditionExpression: "pk = :levelId",
                ExpressionAttributeValues: {
                    ":levelId": "LEVEL#" + levelId
                }
        }));

        // TODO Validation of response

        return queryResponse.Items;
    }

    async getScore(scoreId) {
        var getResponse = await this.dynamoDbClient.send(
            new GetCommand({
                TableName: tableName,
                Key: {
                    pk: `SCORE#${scoreId}`,
                    sk: `SCORE#${scoreId}`
                }
            })
        );

        if (!getResponse?.Item) throw new Error(`Score ${scoreId} not found`);

        return getResponse.Item;
    }
}