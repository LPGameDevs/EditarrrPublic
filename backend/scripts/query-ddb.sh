TABLE=editarrr-level-storage

# Get
# aws dynamodb get-item \
#     --table-name $TABLE \
#     --key '{ "pk": { "S": "LEVEL#4a414edd-da39-4412-8dda-cc484c77966c"}, "sk": { "S": "LEVEL#4a414edd-da39-4412-8dda-cc484c77966c"} }'

# Get First Page of a List
aws dynamodb query \
  --table-name $TABLE \
  --index-name "levelStatus-levelUpdatedAt-index" \
  --select "ALL_PROJECTED_ATTRIBUTES" \
  --limit 10 \
  --no-scan-index-forward \
  --key-condition-expression "levelStatus = :status" \
  --expression-attribute-values '{ ":status": { "S": "PUBLISHED" } }'

# Get the second page
# aws dynamodb query \
#   --table-name $TABLE \
#   --index-name "levelStatus-levelUpdatedAt-index" \
#   --select "ALL_PROJECTED_ATTRIBUTES" \
#   --limit 10 \
#   --no-scan-index-forward \
#   --key-condition-expression "levelStatus = :status" \
#   --expression-attribute-values '{ ":status": { "S": "PUBLISHED" } }' \
#   --exclusive-start-key '{
#         "levelStatus": {
#             "S": "PUBLISHED"
#         },
#         "levelUpdatedAt": {
#             "N": "1698515874383"
#         },
#         "sk": {
#             "S": "LEVEL#4a414edd-da39-4412-8dda-cc484c77966c"
#         },
#         "pk": {
#             "S": "LEVEL#4a414edd-da39-4412-8dda-cc484c77966c"
#         }
#     }'

# Get Total Count
# aws dynamodb query \
#   --table-name $TABLE \
#   --index-name "levelStatus-levelUpdatedAt-index" \
#   --select "COUNT" \
#   --key-condition-expression "levelStatus = :status" \
#   --expression-attribute-values '{ ":status": { "S": "PUBLISHED" } }'