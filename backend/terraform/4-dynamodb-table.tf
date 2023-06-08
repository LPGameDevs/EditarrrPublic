# We use dynamodb to store the level data of the game.

# Create the dynamodb table.
resource "aws_dynamodb_table" "editarrr-level-storage" {
  attribute {
    name = "id"
    type = "S"
  }
  attribute {
    name = "status"
    type = "S"
  }
  attribute {
    name = "lastUpdated"
    type = "S"
  }

  # Add additional indexes.
  global_secondary_index {
    name            = "StatusLastUpdatedIndex"
    hash_key        = "status"
    range_key       = "lastUpdated"
    projection_type = "ALL"
    write_capacity  = 1
    read_capacity   = 1
  }

  billing_mode                = "PAY_PER_REQUEST"
  deletion_protection_enabled = "false"
  hash_key                    = "id"
  name                        = "editarrr-level-storage"

  point_in_time_recovery {
    enabled = "false"
  }

  read_capacity  = "0"
  stream_enabled = "false"
  table_class    = "STANDARD"
  write_capacity = "0"
}

# IAM policy for the lambda to access the dynamodb table.
resource "aws_iam_policy" "dynamoDBLambdaPolicy" {
  name = "DynamoDBLambdaPolicy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "dynamodb:Scan",
          "dynamodb:Query",
          "dynamodb:GetItem",
          "dynamodb:PutItem"
        ]
        Resource = "*"
        #        Resource = [
        #          aws_dynamodb_table.editarrr-level-storage.arn
        #        ]
      }
    ]
  })
}

# Attach the policy to the lambda role.
resource "aws_iam_role_policy_attachment" "lambda-policy-attachment" {
  role       = aws_iam_role.editarrr_lambda_exec.name
  policy_arn = aws_iam_policy.dynamoDBLambdaPolicy.arn
}
