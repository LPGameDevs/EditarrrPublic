# We use dynamodb to store the level data of the game.

# Create the dynamodb table.
resource "aws_dynamodb_table" "editarrr-level-storage" {
  name                        = "editarrr-level-storage"
  billing_mode                = "PAY_PER_REQUEST"
  table_class                 = "STANDARD"
  read_capacity               = "0"
  write_capacity              = "0"
  stream_enabled              = "false"
  deletion_protection_enabled = "false"

  point_in_time_recovery {
    enabled = "false"
  }

  hash_key  = "pk"
  range_key = "sk"
  attribute {
    name = "pk"
    type = "S"
  }
  attribute {
    name = "sk"
    type = "S"
  }

  # Items
  # Note: In the DynamoDB configuration, you can only define attributes that are keyed/indexed
  #       Commenting non-indexed attributes as well for completeness

  # Level Items
  # pk: LEVEL#<levelId>
  # sk: LEVEL#<levelId>
  attribute {
    name = "levelCreatorId"
    type = "S" #userId
  }

  attribute {
    name = "levelStatus"
    type = "S" # PUBLISHED|DRAFT
  }

  attribute {
    name = "levelUpdatedAt"
    type = "N" # Epoch
  }
  # attribute {
  #   name = "levelName"
  #   type = "S"
  # }
  # attribute {
  #   name = "levelCreatedAt"
  #   type = "N" # Epoch
  # }
  # attribute {
  #   name = "levelData"
  #   type = "M" # JSON Blob
  # }

  # Score Items
  # pk: LEVEL#<levelId>
  # sk: SCORE#<score>
  # attribute {
  #   name = "scoreUserId"
  #   type = "S" # userId # optional - only for "logged in" users
  # }
  # attribute {
  #   name = "scorePlayerName"
  #   type = "S" # not necessarily the userName, because it might not be a "logged in" user
  # }
  # attribute {
  #   name = "scoreSubmittedAt"
  #   type = "N" # Epoch
  # }

  # User Items
  # pk: USER#<userId>
  # sk: USER#<userId>
  # attribute {
  #   name = "userName"
  #   type = "S"
  # }

  global_secondary_index {
    name            = "levelCreatorId-levelUpdatedAt-index"
    hash_key        = "levelCreatorId"
    range_key       = "levelUpdatedAt"
    projection_type = "INCLUDE"
    non_key_attributes = [ "pk", "levelName", "levelStatus"]
    write_capacity  = 0
    read_capacity   = 0
  }

  global_secondary_index {
    name            = "levelStatus-levelUpdatedAt-index"
    hash_key        = "levelStatus"
    range_key       = "levelUpdatedAt"
    projection_type = "INCLUDE"
    non_key_attributes = [ "pk", "levelName", "levelCreatorId"]
    write_capacity  = 0
    read_capacity   = 0
  }
}

# IAM policy for the lambda to access the dynamodb table.
resource "aws_iam_policy" "dynamoDBLambdaPolicy" {
  name = "DynamoDBLambdaPolicyEditarrr"

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
