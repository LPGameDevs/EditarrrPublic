#!/bin/bash
set -eo pipefail

TABLE="editarrr-score-storage"

# Step 1: Get all items from the DynamoDB table
items=$(aws dynamodb scan --table-name $TABLE --output json --query 'Items')

# Step 2: Process each item
echo "${items}" | jq -c '.[]' | while IFS= read -r item; do
    # Check if "score" attribute exists in the item
    if echo "${item}" | jq -e 'has("score")' > /dev/null; then
        # Extract the "score" attribute from the item
        score=$(echo "${item}" | jq -r '.score.S')

        # Convert the "score" from string to float
        scoreFloat=$(echo "${score}" | awk '{printf "%.2f", $0}')

        # Delete the "score" attribute
        updatedItem=$(echo "${item}" | jq 'del(.score)')

        # Add the new "scoreNumber" attribute with the float value
        updatedItem=$(echo "${updatedItem}" | jq --arg scoreFloat "${scoreFloat}" '. + {scoreNumber: {N: $scoreFloat}}')

        echo "Making update ${updatedItem}"
        
        # Check if RUN is set to true before executing put-item
        if [ "$RUN" = true ]; then
            # Update the item in the DynamoDB table
            aws dynamodb put-item --table-name $TABLE --item "${updatedItem}" --return-consumed-capacity TOTAL
        fi
    else
        echo "Item does not have a 'score' attribute. Skipping..."
    fi
done
