#!/bin/bash
set -eo pipefail

STACK=editarrr-poc
FUNCTION=$(aws cloudformation describe-stack-resource --stack-name $STACK --logical-resource-id DDBHandlerFunction --query 'StackResourceDetail.PhysicalResourceId' --output text)

cd dynamo-handler
zip function.zip app.mjs
aws lambda update-function-code --function-name $FUNCTION --zip-file fileb://function.zip