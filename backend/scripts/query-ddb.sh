TABLE=editarrr-level-storage

# Get
# aws dynamodb get-item \
#     --table-name $TABLE \
#     --key '{ "pk": { "S": "LEVEL#4a414edd-da39-4412-8dda-cc484c77966c"}, "sk": { "S": "LEVEL#4a414edd-da39-4412-8dda-cc484c77966c"} }'

# Get First Page of a List
# aws dynamodb query \
#   --table-name $TABLE \
#   --index-name "levelStatus-levelUpdatedAt-index" \
#   --select "ALL_PROJECTED_ATTRIBUTES" \
#   --limit 10 \
#   --no-scan-index-forward \
#   --key-condition-expression "levelStatus = :status" \
#   --expression-attribute-values '{ ":status": { "S": "PUBLISHED" } }'

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

# Update Item
# aws dynamodb update-item \
#     --table-name $TABLE \
#     --key '{"pk": {"S": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63"}, "sk": {"S": "LEVEL#5246cf90-7f7f-4074-aea1-ba543d27ed63"}}' \
#     --update-expression "SET levelAvgScore = :avgScore, levelTotalScores = :totalScores" \
#     --expression-attribute-values '{":avgScore": {"N": "1.5"}, ":totalScores": {"N": "1"}}'

# https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.OperatorsAndFunctions.html
# Get Levels with at least one of the queried labels
# aws dynamodb query \
#   --table-name $TABLE \
#   --index-name "levelStatus-levelUpdatedAt-index" \
#   --select "ALL_PROJECTED_ATTRIBUTES" \
#   --limit 10 \
#   --no-scan-index-forward \
#   --key-condition-expression "levelStatus = :status" \
#   --filter-expression "contains(labels, :label1) OR contains(labels, :label2)" \
#   --expression-attribute-values '{ ":status": { "S": "PUBLISHED" }, ":label1": { "S": "test" }, ":label2": { "S": "GDFG" } }'

# https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.OperatorsAndFunctions.html
# Get Levels with at least one of the queried labels AND the levelName is a substring match
# aws dynamodb query \
#   --table-name $TABLE \
#   --index-name "levelStatus-levelUpdatedAt-index" \
#   --select "ALL_PROJECTED_ATTRIBUTES" \
#   --limit 10 \
#   --no-scan-index-forward \
#   --key-condition-expression "levelStatus = :status" \
#   --filter-expression "(contains(labels, :label1) OR contains(labels, :label2)) AND contains(levelName, :nameSubstring)" \
#   --expression-attribute-values '{ ":status": { "S": "PUBLISHED" }, ":label1": { "S": "test" }, ":label2": { "S": "GDFG" }, ":nameSubstring": { "S": "2" } }'