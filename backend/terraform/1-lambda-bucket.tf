# We setup an s3 bucket for the lambda function to use as a storage.

# We use a random pet to generate a random name for the bucket.
resource "random_pet" "lambda_bucket_name" {
  prefix = "lambda"
  length = 2
}

# We create the bucket and set force_destroy to true so that terraform can destroy the bucket when we destroy the infrastructure.
resource "aws_s3_bucket" "lambda_bucket" {
  bucket        = random_pet.lambda_bucket_name.id
  force_destroy = true
}

# We set the public access block to true so that the bucket is public.
resource "aws_s3_bucket_public_access_block" "lambda_bucket" {
  bucket = aws_s3_bucket.lambda_bucket.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}
