#!/bin/bash
set -eo pipefail
APIID=$(aws cloudformation describe-stack-resource --stack-name editarrr-poc --logical-resource-id ServerlessHttpApi --query 'StackResourceDetail.PhysicalResourceId' --output text)
REGION=$(aws configure get region)

URL="https://$APIID.execute-api.$REGION.amazonaws.com"

LEVEL_ID="<LEVEL_ID>"
curl -X "PUT" -H "Content-Type: application/json" -d '{"name": "First Level v2", "status": "published", "levelData": {"foo": "bar2"}}' ${URL}/levels/${LEVEL_ID} | python3 -m json.tool