#!/bin/bash
set -eo pipefail

APIID="ygfmolwgd9"
REGION="eu-north-1"

# If using CloudFormation:
# APIID=$(aws cloudformation describe-stack-resource --stack-name editarrr-poc --logical-resource-id ServerlessHttpApi --query 'StackResourceDetail.PhysicalResourceId' --output text)
# REGION=$(aws configure get region)

export URL="https://$APIID.execute-api.$REGION.amazonaws.com/dev/levels"