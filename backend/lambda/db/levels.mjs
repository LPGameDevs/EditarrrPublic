import {
    GetCommand, UpdateCommand, QueryCommand
} from "@aws-sdk/lib-dynamodb";
import { BadRequestException, isArrayOfStrings } from "../utils.mjs";

const tableNameLevel = "editarrr-level-storage";

const indexStatusUpdatedAt = "levelStatus-levelUpdatedAt-index";
const indexStatusCreatedAt = "levelStatus-levelCreatedAt-index";
const indexStatusAvgScore = "levelStatus-levelAvgScore-index"
const indexStatusTotalScores = "levelStatus-levelTotalScores-index";
const indexStatusAvgRating = "levelStatus-levelAvgRating-index";
const indexStatusTotalRatings = "levelStatus-levelTotalRatings-index";

// Because JS doesn't have enums...
export class LevelsSortOptions {
    static UPDATED_AT = "updated-at";
    static CREATED_AT = "created-at";
    static AVG_SCORE = "avg-score";
    static TOTAL_SCORES = "total-scores";
    static AVG_RATING = "avg-rating";
    static TOTAL_RATINGS = "total-ratings";

    static isValid(str) {
        for (const validOption in LevelsSortOptions) {
            if (LevelsSortOptions[validOption] === str) {
                return true;
            }
        }
        return false;
    }

    static getAllValidValues() {
        return Object.values(LevelsSortOptions);
    }
}

const sortOptionToIndex = {
    [LevelsSortOptions.UPDATED_AT]: indexStatusUpdatedAt,
    [LevelsSortOptions.CREATED_AT]: indexStatusCreatedAt,
    [LevelsSortOptions.AVG_SCORE]: indexStatusAvgScore,
    [LevelsSortOptions.TOTAL_SCORES]: indexStatusTotalScores,
    [LevelsSortOptions.AVG_RATING]: indexStatusAvgRating,
    [LevelsSortOptions.TOTAL_RATINGS]: indexStatusTotalRatings,
}

const sortOptionToAttributeName = {
    [LevelsSortOptions.UPDATED_AT]: "levelUpdatedAt",
    [LevelsSortOptions.CREATED_AT]: "levelCreatedAt",
    [LevelsSortOptions.AVG_SCORE]: "levelAvgScore",
    [LevelsSortOptions.TOTAL_SCORES]: "levelTotalScores",
    [LevelsSortOptions.AVG_RATING]: "levelAvgRating",
    [LevelsSortOptions.TOTAL_RATINGS]: "levelTotalRatings",
}

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

    async getPagedLevels(
        sortOption,
        sortAsc,
        pageLimit, 
        cursor,
        useDrafts, 
        filters,
    ) {
        var query = {
            TableName: tableNameLevel,
            IndexName: sortOptionToIndex[sortOption],
            Select: "ALL_PROJECTED_ATTRIBUTES",
            Limit: pageLimit,
            ScanIndexForward: sortAsc,
            KeyConditionExpression: "levelStatus = :status",
            ExpressionAttributeValues: {
                ":status": useDrafts ? "DRAFT" : "PUBLISHED"
            }
        }

        if (cursor) {
            // Using levelId as a cursor, we have to fetch more level data in order to provide DDB with all the data it expects from the cursor
            var cursorLevelData = await this.getLevel(cursor);
            
            var sortedAttributeName = sortOptionToAttributeName[sortOption];

            query.ExclusiveStartKey = {
                levelStatus: cursorLevelData.levelStatus,
                [sortedAttributeName]: cursorLevelData[sortedAttributeName],
                pk: cursorLevelData.pk,
                sk: cursorLevelData.sk,
            };
        }

        if (filters?.anyOfLabels !== undefined) {
            if (!isArrayOfStrings) throw new BadRequestException(`'anyOfLabels' must be a comma-separated list of strings. E.g. 'anyOfLabels=test,GDFG'`);

            var anyOfLabels = filters?.anyOfLabels;
            if (anyOfLabels.length > 0) {
                query.FilterExpression = anyOfLabels.map((label, index) => `contains(labels, :label${index})`).join(" OR ");
                query.ExpressionAttributeValues = anyOfLabels.reduce((values, label, index) => {
                    values[`:label${index}`] = label;
                    return values;
                }, query.ExpressionAttributeValues);
            }
        }

        var queryResponse = await this.dynamoDbClient.send(new QueryCommand(query));

        // TODO Validation of response

        return {
            levels: queryResponse.Items,
            cursor: queryResponse.LastEvaluatedKey,
        };
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

    async updateLevelRatingData(levelId, avgRating, totalNumberOfRatings) {
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
                UpdateExpression: "SET levelAvgRating = :avgRating, levelTotalRatings = :totalRatings",
                ExpressionAttributeValues: {
                  ":avgRating": avgRating,
                  ":totalRatings": totalNumberOfRatings,
                },
            })
        );
    }
}
    