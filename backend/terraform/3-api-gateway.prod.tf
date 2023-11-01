# AWS API Gateway V2 to make requests to AWS Lambda
resource "aws_apigatewayv2_api" "prod_editarrr_api" {
  name          = "prod_editarrr_api"
  protocol_type = "HTTP"

  cors_configuration {
    allow_headers = ["*"]
    allow_methods = ["*"]
    allow_origins = ["*"]
  }
}

# Productions stage for release.
resource "aws_apigatewayv2_stage" "prod" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  name        = "prod"
  auto_deploy = true

  # What data to log to cloudwatch.
  access_log_settings {
    destination_arn = aws_cloudwatch_log_group.prod_editarrr_api_logs.arn

    format = jsonencode({
      requestId               = "$context.requestId"
      sourceIp                = "$context.identity.sourceIp"
      requestTime             = "$context.requestTime"
      protocol                = "$context.protocol"
      httpMethod              = "$context.httpMethod"
      resourcePath            = "$context.resourcePath"
      routeKey                = "$context.routeKey"
      status                  = "$context.status"
      responseLength          = "$context.responseLength"
      integrationErrorMessage = "$context.integrationErrorMessage"
    })
  }
}

# We create a log group for the lambda function to use.
resource "aws_cloudwatch_log_group" "prod_editarrr_api_logs" {
  name = "/aws/api-gw/${aws_apigatewayv2_api.prod_editarrr_api.name}"

  retention_in_days = 14
}

# Connect the lambda function to the API Gateway.
resource "aws_apigatewayv2_integration" "prod_editarrr_lambda_integration" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  integration_uri    = aws_lambda_function.prod_editarrr_lambda.invoke_arn
  integration_type   = "AWS_PROXY"
  integration_method = "POST"
}

# Create an api route for fetching all levels.
resource "aws_apigatewayv2_route" "prod_get_levels" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "GET /levels"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for fetching a level by id.
resource "aws_apigatewayv2_route" "prod_get_level_by_id" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "GET /levels/{id}"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for creating a level.
resource "aws_apigatewayv2_route" "prod_post_level" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "POST /levels"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for updating a level.
resource "aws_apigatewayv2_route" "prod_put_level" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "PATCH /levels/{id}"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for uploading a screenshot to s3.
resource "aws_apigatewayv2_route" "prod_put_screenshot" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "POST /screenshot/{filename}"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for fetching all scores for a level.
resource "aws_apigatewayv2_route" "prod_get_scores" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "GET /levels/{id}/scores"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for adding a score for a level.
resource "aws_apigatewayv2_route" "prod_post_score" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "POST /levels/{id}/scores"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for fetching all ratings for a level.
resource "aws_apigatewayv2_route" "prod_get_ratings" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "GET /levels/{id}/ratings"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for adding a rating for a level.
resource "aws_apigatewayv2_route" "prod_post_rating" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "POST /levels/{id}/ratings"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for adding a rating for a level.
resource "aws_apigatewayv2_route" "prod_post_analytics" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "POST /analytics"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for adding a rating for a level.
resource "aws_apigatewayv2_route" "prod_get_user_analytics" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "GET /user/{id}/analytics"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

# Create an api route for adding a rating for a level.
resource "aws_apigatewayv2_route" "prod_get_type_analytics" {
  api_id = aws_apigatewayv2_api.prod_editarrr_api.id

  route_key = "GET /analytics/{type}"
  target    = "integrations/${aws_apigatewayv2_integration.prod_editarrr_lambda_integration.id}"
}

resource "aws_lambda_permission" "prod_api_gateway_permission" {
  statement_id  = "AllowProdExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.prod_editarrr_lambda.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_apigatewayv2_api.prod_editarrr_api.execution_arn}/*/*"
}

output "prod_base_url" {
  value = aws_apigatewayv2_stage.prod.invoke_url
}
