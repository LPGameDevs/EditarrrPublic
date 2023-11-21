#!/bin/bash
set -eo pipefail

# NOTE: This script will do a dry run unless you set 'RUN=true'

LEVEL_TABLE="editarrr-level-storage"
SCORE_TABLE="editarrr-score-storage"

echo_red() {
    echo -e "\e[31m$1\e[0m"
}

echo_yellow() {
    echo -e "\e[33m$1\e[0m"
}


# Step 1: Get all 'level' items from the DynamoDB table
levels=$(aws dynamodb scan --table-name $LEVEL_TABLE --output json --query 'Items')

# Step 2: Process each 'level' item
echo "${levels}" | jq -c '.[]' | while IFS= read -r levelItem; do
    # Extract relevant attributes from the 'level' item
    levelPk=$(echo "${levelItem}" | jq -r '.pk.S')
    levelName=$(echo "${levelItem}" | jq -r '.levelName.S')
    levelStatus=$(echo "${levelItem}" | jq -r '.levelStatus.S')
    levelUpdatedAt=$(echo "${levelItem}" | jq -r '.levelUpdatedAt.N')

    echo "Checking level $levelName (pk: $levelPk, levelStatus: $levelStatus, levelUpdatedAt: $levelUpdatedAt) for scores..."

    # Step 3: Fetch all 'score' items for the current 'level'
    scores=$(aws dynamodb query --table-name $SCORE_TABLE \
                --key-condition-expression "pk = :pk" \
                --expression-attribute-values '{":pk": {"S": "'$levelPk'"}}' \
                --output json \
                --query 'Items')

    # Step 4: Process each 'score' item for the current 'level'
    echo "${scores}" | jq -c '.[]' | while IFS= read -r scoreItem; do
        scoreSk=$(echo "${scoreItem}" | jq -r '.sk.S')
        scoreCreatorName=$(echo "${scoreItem}" | jq -r '.scoreCreatorName.S')
        scoreNumber=$(echo "${scoreItem}" | jq -r '.scoreNumber.N')
        scoreSubmittedAt=$(echo "${scoreItem}" | jq -r '.scoreSubmittedAt.N')

        # Step 5: Check if the 'level' is in DRAFT state or has been updated
        if [ "$levelStatus" = "DRAFT" ]; then
            if [ "$RUN" = true ]; then
                aws dynamodb delete-item --table-name $SCORE_TABLE --key '{"pk": {"S": "'$levelPk'"}, "sk": {"S": "'$scoreSk'"}}'
                echo_red "Deleted 'score' $scoreNumber for $scoreCreatorName on level $levelName because the level is a DRAFT"
            else
                echo_yellow "Dry Run: Would delete 'score' $scoreNumber for $scoreCreatorName on level $levelName because the level is a DRAFT"
            fi
        elif ([ "$levelUpdatedAt" ] && [ "$scoreSubmittedAt" -lt "$levelUpdatedAt" ]); then
            # Step 6: Delete the 'score' item for the current 'level' if it meets the criteria
            if [ "$RUN" = true ]; then
                aws dynamodb delete-item --table-name $SCORE_TABLE --key '{"pk": {"S": "'$levelPk'"}, "sk": {"S": "'$scoreSk'"}}'
                echo_red "Deleted 'score' $scoreNumber for $scoreCreatorName on level $levelName because the score was submitted before the level was published"
            else
                echo_yellow "Dry Run: Would delete 'score' $scoreNumber for $scoreCreatorName on level $levelName because the score was submitted $scoreSubmittedAt (before the level was published)"
            fi
        else
            echo "'score' $scoreNumber for $scoreCreatorName on level $levelName is legitimate"
        fi
    done

    echo
done
