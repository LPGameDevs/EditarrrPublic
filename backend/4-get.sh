#!/bin/bash
set -eo pipefail
APIID=$(aws cloudformation describe-stack-resource --stack-name editarrr-poc --logical-resource-id ServerlessHttpApi --query 'StackResourceDetail.PhysicalResourceId' --output text)
REGION=$(aws configure get region)

curl https://$APIID.execute-api.$REGION.amazonaws.com/items