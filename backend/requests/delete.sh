#!/bin/bash
set -eo pipefail
APIID=$(aws cloudformation describe-stack-resource --stack-name editarrr-poc --logical-resource-id ServerlessHttpApi --query 'StackResourceDetail.PhysicalResourceId' --output text)
REGION=$(aws configure get region)

LEVEL_TO_DELETE="<LEVEL_ID>"
curl -X "DELETE" https://$APIID.execute-api.$REGION.amazonaws.com/levels/${LEVEL_TO_DELETE} | python3 -m json.tool