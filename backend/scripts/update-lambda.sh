#!/bin/bash
set -eo pipefail

# NOTE: Ideally, use 'terraform apply' script
# Only using this because terraform isn't working for me

LAMBDA_NAME=editarrr-lambda-function
LAMBDA_ZIP_NAME=editarrr-lambda-function.zip

(cd lambda && zip -r ../${LAMBDA_ZIP_NAME} .)

aws lambda update-function-code \
    --function-name ${LAMBDA_NAME} \
    --zip-file fileb://${LAMBDA_ZIP_NAME}