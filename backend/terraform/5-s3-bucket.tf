resource "random_pet" "screenshot_bucket_name" {
  prefix = "editarrr-screenshot"
  length = 2
}

resource "aws_s3_bucket" "screenshot_bucket" {
  bucket        = random_pet.screenshot_bucket_name.id
}

resource "aws_s3_bucket_ownership_controls" "screenshot_bucket" {
  bucket = aws_s3_bucket.screenshot_bucket.id
  rule {
    object_ownership = "BucketOwnerPreferred"
  }
}

resource "aws_s3_bucket_public_access_block" "screenshot_bucket" {
  bucket = aws_s3_bucket.screenshot_bucket.id

  block_public_acls       = false
  block_public_policy     = false
  ignore_public_acls      = false
  restrict_public_buckets = false
}

resource "aws_s3_bucket_acl" "screenshot_bucket" {
  depends_on = [
    aws_s3_bucket_ownership_controls.screenshot_bucket,
    aws_s3_bucket_public_access_block.screenshot_bucket,
  ]

  bucket = aws_s3_bucket.screenshot_bucket.id
  acl    = "public-read"
}


# IAM policy for the lambda to access the dynamodb table.

resource "aws_iam_policy" "s3LambdaPolicy" {
  name = "S3LambdaPolicyEditarrr"

  policy = jsonencode({
    Version   = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "s3:PutObject",
          "s3:GetObject",
          "s3:*", # PutObject is enough for normal upload but not WITH ACL.
        ]
        Resource = "*"
        #        Resource = [
        #          aws_s3_bucket.screenshot_bucket.arn,
        #        ]
      }
    ]
  })
}

# Attach the policy to the lambda role.
resource "aws_iam_role_policy_attachment" "s3-policy-attachment" {
  role       = aws_iam_role.editarrr_lambda_exec.name
  policy_arn = aws_iam_policy.s3LambdaPolicy.arn
}
