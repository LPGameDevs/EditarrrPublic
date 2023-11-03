import { expect } from 'chai';
import { stub } from 'sinon';

import { DynamoDBDocumentClient } from '@aws-sdk/lib-dynamodb';
import { DynamoDBClient } from '@aws-sdk/client-dynamodb';

import { LevelsApi } from './levels.mjs';
import { LevelsDbClient } from '../db/levels.mjs';

var dynamoDbClient;
var levelsDbClient;
var levelsApi;

describe('GetLevel', function () {
    
    beforeEach(function () {
        dynamoDbClient = new DynamoDBDocumentClient(new DynamoDBClient({}));
        levelsDbClient = new LevelsDbClient(dynamoDbClient);
        levelsApi = new LevelsApi(levelsDbClient);
    });

    it('should fetch and translate a level from the db."', async function () {
        const ddbClientSendStub = stub(dynamoDbClient, 'send');
        ddbClientSendStub.returns({
            "Item": {
                "levelName": "65758",
                "levelUpdatedAt": 1698515874383,
                "levelCreatorId": "0f785537-b000-4b4f-a349-6ac781460681",
                "levelData": {},
                "levelStatus": "PUBLISHED",
                "sk": "LEVEL#4a414edd-da39-4412-8dda-cc484c77966c",
                "levelCreatedAt": 1698514557729,
                "pk": "LEVEL#4a414edd-da39-4412-8dda-cc484c77966c",
                "levelCreatorName": "Fresch"
            }
        });
        
        const result = await levelsApi.getLevel();
        expect(result).to.deep.equal({
            "id": "4a414edd-da39-4412-8dda-cc484c77966c",
            "name": "65758",
            "creator": {
                "id": "0f785537-b000-4b4f-a349-6ac781460681",
                "name": "Fresch",
            },
            "updatedAt": 1698515874383,
            "data": {},
            "status": "PUBLISHED",
            "createdAt": 1698514557729,
    
        });
    });
});