#!/bin/bash
set -eo pipefail

docker run \
    --name dynamodb \
    -p 8000:8000 \
    -d amazon/dynamodb-local \
    -jar DynamoDBLocal.jar