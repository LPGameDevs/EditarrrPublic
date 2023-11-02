# We create a lambda function which will decide what to do with HTTP requests.

# The lambda function itself is a simple nodejs script.
resource "aws_lambda_function" "prod_editarrr_lambda" {
  function_name = "prod-editarrr-lambda-function"

  s3_bucket = aws_s3_bucket.lambda_bucket.id
  # the s3 key is the name of the zip file we uploaded.
  s3_key    = aws_s3_object.editarrr_lambda_zip.key

  runtime = "nodejs18.x"
  handler = "function_prod.handler"

  source_code_hash = data.archive_file.editarrr_lambda_zip_data.output_base64sha256

  role = aws_iam_role.editarrr_lambda_exec.arn
}
