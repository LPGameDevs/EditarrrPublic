#!/bin/bash
set -eo pipefail

# Steps
# 1. Set up mock AWS creds + config
# 2. Start the local DynamoDB
# 3. Set up the table for the DynamoDB
# 4. Start the Nodejs lambda (npm install, npm run...)
# 5. Make a confirmation query

DDB="dynamodb"

if [ ! -d ~/.aws ]; then 
    echo "Setting up mock AWS config & creds..."
    mkdir -p ~/.aws
    echo -e "[default]\nregion = eu-north-1" > ~/.aws/config
    echo -e "[default]\naws_access_key_id = MOCK_ACCESS_KEY\naws_secret_access_key = MOCK_SECRET_KEY" > ~/.aws/credentials; 
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
            -p 8000:8000 \
            -d amazon/dynamodb-local \
            -jar DynamoDBLocal.jar
    fi
fi

# TODO Set up the DDB table

# TODO Start the Nodejs lambda

# TODO Make a confirmation query