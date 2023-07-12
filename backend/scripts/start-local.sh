#!/bin/bash
set -eo pipefail

# Steps
# 1. Set up mock AWS creds + config
# 2. Start the local DynamoDB
# 3. Set up the table for the DynamoDB
# 4. Start the Nodejs lambda (npm install, npm run...)
# 5. Make a confirmation query

PORT=8000
DDB="dynamodb"

if [ ! -d ~/.aws ]; then 
    echo "Setting up mock AWS config & creds..."
    mkdir -p ~/.aws
    echo -e "[default]\nregion = eu-north-1" > ~/.aws/config
    echo -e "[default]\naws_access_key_id=fakeAccessKey\naws_secret_access_key=fakeSecretKey" > ~/.aws/credentials; 
fi

if docker ps -f "name=$DDB" --format '{{.Names}}' | grep -q "^$DDB$"; then
    echo "Container '$DDB' is already running."
else
    if docker ps -a -f "name=$DDB" --format '{{.Status}}' | grep -q "^Exited"; then
        echo "Container '$DDB' exists but is exited. Restarting..."
        docker start $DDB
    else
        docker run \
            --name dynamodb \
            -p ${PORT}:${PORT} \
            -d amazon/dynamodb-local \
            -jar DynamoDBLocal.jar
    fi
fi

# TODO Set up the DDB table (if it isn't set up already)
aws dynamodb create-table \
    --table-name editarrr-level-storage \
    --attribute-definitions \
        AttributeName=pk,AttributeType=S \
        AttributeName=sk,AttributeType=S \
        AttributeName=levelCreatorId,AttributeType=S \
        AttributeName=levelStatus,AttributeType=S \
        AttributeName=levelUpdatedAt,AttributeType=N \
    --key-schema \
        AttributeName=pk,KeyType=HASH \
        AttributeName=sk,KeyType=RANGE \
    --global-secondary-indexes \
        "[
            {
                \"IndexName\": \"levelCreatorId-levelUpdatedAt-index\",
                \"KeySchema\": [{\"AttributeName\":\"levelCreatorId\",\"KeyType\":\"HASH\"},
                                {\"AttributeName\":\"levelUpdatedAt\",\"KeyType\":\"RANGE\"}],
                \"Projection\":{
                    \"ProjectionType\":\"INCLUDE\",
                    \"NonKeyAttributes\":[\"pk\",\"levelName\",\"levelStatus\"]
                },
                \"ProvisionedThroughput\": {
                    \"ReadCapacityUnits\": 1,
                    \"WriteCapacityUnits\": 1
                }
            },
            {
                \"IndexName\": \"levelStatus-levelUpdatedAt-index\",
                \"KeySchema\": [{\"AttributeName\":\"levelStatus\",\"KeyType\":\"HASH\"},
                                {\"AttributeName\":\"levelUpdatedAt\",\"KeyType\":\"RANGE\"}],
                \"Projection\":{
                    \"ProjectionType\":\"INCLUDE\",
                    \"NonKeyAttributes\":[\"pk\",\"levelName\",\"levelCreatorId\"]
                },
                \"ProvisionedThroughput\": {
                    \"ReadCapacityUnits\": 1,
                    \"WriteCapacityUnits\": 1
                }
            }
        ]" \
    --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1 \
    --endpoint-url http://localhost:${PORT}

# aws dynamodb list-tables --endpoint-url http://localhost:${PORT}
# aws dynamodb delete-table --table-name <table-name> --endpoint-url http://localhost:${PORT}


# TODO Start the Nodejs lambda

# TODO Make a confirmation query