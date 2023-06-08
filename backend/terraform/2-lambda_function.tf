# We create a lambda function which will decide what to do with HTTP requests.

# Create an IAM role for the lambda function to use.
resource "aws_iam_role" "editarrr_lambda_exec" {
  name = "editarrr-lambda-role"
  assume_role_policy = <<POLICY
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "lambda.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
POLICY
}

# The lambda function itself is a simple nodejs script.
resource "aws_lambda_function" "editarrr_lambda" {
  function_name = "editarrr-lambda-function"

  s3_bucket = aws_s3_bucket.lambda_bucket.id
  # the s3 key is the name of the zip file we uploaded.
  s3_key    = aws_s3_object.editarrr_lambda_zip.key

  runtime = "nodejs18.x"
  handler = "function.handler"

  source_code_hash = data.archive_file.editarrr_lambda_zip_data.output_base64sha256

  role = aws_iam_role.editarrr_lambda_exec.arn
}

# Simple script to zip the lambda function.
data "archive_file" "editarrr_lambda_zip_data" {
  type = "zip"

  source_dir  = "../${path.module}/lambda"
  output_path = "../${path.module}/lambda.zip"
}

# We upload the lambda function to S3.
resource "aws_s3_object" "editarrr_lambda_zip" {
  bucket = aws_s3_bucket.lambda_bucket.id

  key    = "lambda.zip"
  source = data.archive_file.editarrr_lambda_zip_data.output_path

  etag = filemd5(data.archive_file.editarrr_lambda_zip_data.output_path)
}
