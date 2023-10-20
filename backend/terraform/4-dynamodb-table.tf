# We use dynamodb to store the level data of the game.

# Create the dynamodb table for levels.
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
    non_key_attributes = [ "pk", "levelName", "levelStatus", "levelCreatorName"]
    write_capacity  = 0
    read_capacity   = 0
  }

  global_secondary_index {
    name            = "levelStatus-levelUpdatedAt-index"
    hash_key        = "levelStatus"
    range_key       = "levelUpdatedAt"
    projection_type = "INCLUDE"
    non_key_attributes = [ "pk", "levelName", "levelCreatorId", "levelCreatorName"]
    write_capacity  = 0
    read_capacity   = 0
  }
}

# Create the dynamodb table for scores.
resource "aws_dynamodb_table" "editarrr-score-storage" {
  name                        = "editarrr-score-storage"
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

  # Score Items
  # pk: LEVEL#<levelId>
  # sk: SCORE#<score>

  attribute {
    name = "score"
    type = "S" # Number of seconds 0015.123
  }

#  attribute {
#    name = "scoreLevelName"
#    type = "S" # 12345
#  }
#  attribute {
#    name = "scoreSubmittedAt"
#    type = "N" # Epoch
#  }
#  attribute {
#    name = "scoreCreatorId"
#    type = "S" #userId
#  }
  # attribute {
  #   name = "scoreCreatorName"
  #   type = "S"
  # }
  # attribute {
  #   name = "ghost"
  #   type = "M" # JSON Blob
  # }

  global_secondary_index {
    name            = "scoreLevelName-score-index"
    hash_key        = "pk"
    range_key       = "score"
    projection_type = "INCLUDE"
    non_key_attributes = [ "sk", "pk", "scoreLevelName", "scoreCreatorName", "scoreSubmittedAt", "scoreCreatorId"]
    write_capacity  = 0
    read_capacity   = 0
  }
}

# Create the dynamodb table for ratings.
resource "aws_dynamodb_table" "editarrr-rating-storage" {
  name                        = "editarrr-rating-storage"
  billing_mode                = "PAY_PER_REQUEST"
  table_class                 = "STANDARD"
  read_capacity               = "0"
  write_capacity              = "0"
  stream_enabled              = "false"
  deletion_protection_enabled = "false"

  point_in_time_recovery {
    enabled = "false"
  }

  hash_key  = "pk" # LEVEL#<levelId>
  range_key = "sk" # RATING#<rating>
  attribute {
    name = "pk"
    type = "S"
  }
  attribute {
    name = "sk"
    type = "S"
  }
  attribute {
    name = "rating"
    type = "N" # 1, 2 or 3
  }

  #  attribute {
  #    name = "ratingLevelName"
  #    type = "S" # 12345
  #  }
  #  attribute {
  #    name = "ratingSubmittedAt"
  #    type = "N" # Epoch
  #  }
  #  attribute {
  #    name = "ratingCreatorId"
  #    type = "S" #userId
  #  }
  # attribute {
  #   name = "ratingCreatorName"
  #   type = "S" # userName
  # }

  global_secondary_index {
    name            = "pk-rating-index"
    hash_key        = "pk"
    range_key       = "rating"
    projection_type = "INCLUDE"
    non_key_attributes = [ "sk", "ratingLevelName", "ratingCreatorName", "ratingSubmittedAt", "ratingCreatorId"]
    write_capacity  = 0
    read_capacity   = 0
  }
}

# Create the dynamodb table for analytics.
resource "aws_dynamodb_table" "editarrr-analytics-storage" {
  name                        = "editarrr-analytics-storage"
  billing_mode                = "PAY_PER_REQUEST"
  table_class                 = "STANDARD"
  read_capacity               = "0"
  write_capacity              = "0"
  stream_enabled              = "false"
  deletion_protection_enabled = "false"

  point_in_time_recovery {
    enabled = "false"
  }

  hash_key  = "pk" # USER#<userId>
  range_key = "sk" # ANALYTICS#<analytics>
  attribute {
    name = "pk"
    type = "S"
  }
  attribute {
    name = "sk"
    type = "S"
  }
  attribute {
    name = "type"
    type = "S" # Enum
  }
  attribute {
    name = "value"
    type = "S" # Various
  }

  #  attribute {
  #    name = "analyticsSubmittedAt"
  #    type = "N" # Epoch
  #  }
  # attribute {
  #   name = "creatorName"
  #   type = "S" # userName
  # }

  global_secondary_index {
    name            = "pk-sk-index"
    hash_key        = "pk"
    range_key       = "sk"
    projection_type = "INCLUDE"
    non_key_attributes = [ "type", "value", "userName", "analyticsSubmittedAt" ]
    write_capacity  = 0
    read_capacity   = 0
  }
  
    global_secondary_index {
      name            = "type-pk-index"
      hash_key        = "type"
      range_key       = "pk"
      projection_type = "INCLUDE"
      non_key_attributes = [ "sk", "value", "userName", "analyticsSubmittedAt" ]
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
