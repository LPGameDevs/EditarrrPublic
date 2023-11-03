#!/bin/bash
set -eo pipefail

# If using CloudFormation:
# APIID=$(aws cloudformation describe-stack-resource --stack-name editarrr-poc --logical-resource-id ServerlessHttpApi --query 'StackResourceDetail.PhysicalResourceId' --output text)
# REGION=$(aws configure get region)
APIID="tlfb41owe5"
REGION="eu-north-1"

if [[ $LOCAL == "true" ]]; then
    export URL="http://localhost:3000/levels"
else
    export URL="https://$APIID.execute-api.$REGION.amazonaws.com/dev/levels"
fi

echo "Querying ${URL}..."