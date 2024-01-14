import {
    GetCommand, UpdateCommand, QueryCommand
} from "@aws-sdk/lib-dynamodb";
import { andExpression } from "../utils.mjs";

const tableNameLevel = "editarrr-level-storage";

const indexStatusUpdatedAt = "levelStatus-levelUpdatedAt-index";
const indexStatusCreatedAt = "levelStatus-levelCreatedAt-index";
const indexStatusAvgScore = "levelStatus-levelAvgScore-index"
const indexStatusTotalScores = "levelStatus-levelTotalScores-index";
const indexStatusAvgRating = "levelStatus-levelAvgRating-index";
const indexStatusTotalRatings = "levelStatus-levelTotalRatings-index";
const indexStatusLevelName = "levelStatus-levelName-index";
const indexStatusLevelCreatorName = "levelStatus-levelCreatorName-index";

// Because JS doesn't have enums...
export class LevelsSortOptions {
    static UPDATED_AT = "updated-at";
    static CREATED_AT = "created-at";
    static AVG_SCORE = "avg-score";
    static TOTAL_SCORES = "total-scores";
    static AVG_RATING = "avg-rating";
    static TOTAL_RATINGS = "total-ratings";
    static LEVEL_CODE = "level-code";
    static CREATOR_NAME = "creator-name";

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
    [LevelsSortOptions.LEVEL_CODE]: indexStatusLevelName,
    [LevelsSortOptions.CREATOR_NAME]: indexStatusLevelCreatorName,
}

const sortOptionToAttributeName = {
    [LevelsSortOptions.UPDATED_AT]: "levelUpdatedAt",
    [LevelsSortOptions.CREATED_AT]: "levelCreatedAt",
    [LevelsSortOptions.AVG_SCORE]: "levelAvgScore",
    [LevelsSortOptions.TOTAL_SCORES]: "levelTotalScores",
    [LevelsSortOptions.AVG_RATING]: "levelAvgRating",
    [LevelsSortOptions.TOTAL_RATINGS]: "levelTotalRatings",
    [LevelsSortOptions.LEVEL_CODE]: "levelName",
    [LevelsSortOptions.CREATOR_NAME]: "levelCreatorName"
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

        // We only set a limit if there are filters because
        // if we have a FilterExpression in a DDB query, we can NOT use 'limit' for pagination.
        // The filtering doesn't happen until _after_ the first page is fetched - meaning it only filters on the limited set of items. 
        // From the AWS Doc (https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Query.FilterExpression.html)
        // 'A Query operation can retrieve a maximum of 1 MB of data. This limit applies before the filter expression is evaluated.'
        // and see this discussion: https://stackoverflow.com/questions/40138551/how-can-i-do-dynamodb-limit-after-filtering
        // Note: the '--max-items' flag in the CLI does what we want, but it is a CLI-only option (https://docs.aws.amazon.com/cli/latest/reference/dynamodb/query.html)
        if (filters === undefined || Object.keys(filters).length == 0) {
            query.Limit = pageLimit
        }

        var anyOfLabels = filters?.anyOfLabels;
        if (anyOfLabels !== undefined && anyOfLabels.length > 0) {
            var containsAnyOfLabelsFilterExpression = anyOfLabels.map((label, index) => `contains(labels, :label${index})`).join(" OR ");
            query.FilterExpression = andExpression(query.FilterExpression, containsAnyOfLabelsFilterExpression);
            query.ExpressionAttributeValues = anyOfLabels.reduce((values, label, index) => {
                values[`:label${index}`] = label;
                return values;
            }, query.ExpressionAttributeValues);
        }

        var nameContains = filters?.nameContains;
        if (nameContains !== undefined) {
            query.FilterExpression = andExpression(query.FilterExpression, 'contains(levelName, :nameSubstring)')
            query.ExpressionAttributeValues[':nameSubstring'] = nameContains;
        }

        var queryResponse = await this.dynamoDbClient.send(new QueryCommand(query));

        // See comments above why we have to do this
        if (filters !== undefined && Object.keys(filters).length > 0) {
            if (queryResponse.Items.length > pageLimit) {
                queryResponse.Items = queryResponse.Items.slice(0, pageLimit);
                queryResponse.LastEvaluatedKey = queryResponse.Items[pageLimit-1];
            }
        }

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
    