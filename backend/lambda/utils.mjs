import crypto from "crypto";

export class BadRequestException extends Error {
    constructor(message) {
        super(`error - Bad Request: ${message}`);
        this.name = this.constructor.name;
    }
}

export function asBool(str) {
    switch(str) {
        case "true":
            return true
        case "false":
            return false
        default:
            return undefined
    } 
}

// Pulls out the ID of a str of the format '<STR_TYPE>#<ID?'
export function extractId(ddbKeyStr) {
    const splitDDBLevelKeyStr = ddbKeyStr.match(/#([0-9a-f-]+)/i);

    if (!splitDDBLevelKeyStr) {
        throw new Error(`problem parsing database ID`)
    }

    return splitDDBLevelKeyStr[1];
}

// From https://stackoverflow.com/questions/105034/how-do-i-create-a-guid-uuid - not sure if this is reliable haha
export function uuidv4() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}