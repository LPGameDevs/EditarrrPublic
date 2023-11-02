export function extractLevelId(ddbLevelKeyStr) {
    const splitDDBLevelKeyStr = ddbLevelKeyStr.match(/#([0-9a-f-]+)/i);

    if (!splitDDBLevelKeyStr) {
        throw new Error(`problem parsing database ID`)
    }

    return splitDDBLevelKeyStr[1];
}