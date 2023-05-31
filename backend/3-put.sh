#!/bin/bash
set -eo pipefail
APIID=$(aws cloudformation describe-stack-resource --stack-name editarrr-poc --logical-resource-id ServerlessHttpApi --query 'StackResourceDetail.PhysicalResourceId' --output text)
REGION=$(aws configure get region)

URL="https://$APIID.execute-api.$REGION.amazonaws.com"

curl -X "PUT" -H "Content-Type: application/json" -d "{\"id\": \"456\", \"price\": 12345, \"name\": \"myitem\"}" ${URL}/items