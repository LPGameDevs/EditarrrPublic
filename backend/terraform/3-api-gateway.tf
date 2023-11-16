# AWS API Gateway V2 to make requests to AWS Lambda
resource "aws_apigatewayv2_api" "main" {
  name          = "main"
  protocol_type = "HTTP"

  cors_configuration {
    allow_headers = ["*"]
    allow_methods = ["*"]
    allow_origins = ["*"]
  }
}

# Dev stage for development. Other stages can be created for production, staging, etc.
resource "aws_apigatewayv2_stage" "dev" {
  api_id = aws_apigatewayv2_api.main.id

  name        = "dev"
  auto_deploy = true

  # What data to log to cloudwatch.
  access_log_settings {
    destination_arn = aws_cloudwatch_log_group.main_api_logs.arn

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
resource "aws_cloudwatch_log_group" "main_api_logs" {
  name = "/aws/api-gw/${aws_apigatewayv2_api.main.name}"

  retention_in_days = 14
}

# Connect the lambda function to the API Gateway.
resource "aws_apigatewayv2_integration" "editarrr_lambda_integration" {
  api_id = aws_apigatewayv2_api.main.id

  integration_uri    = aws_lambda_function.editarrr_lambda.invoke_arn
  integration_type   = "AWS_PROXY"
  integration_method = "POST"
}

# Create an api route for fetching all levels.
resource "aws_apigatewayv2_route" "get_levels" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "GET /levels"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for fetching a level by id.
resource "aws_apigatewayv2_route" "get_level_by_id" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "GET /levels/{id}"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for creating a level.
resource "aws_apigatewayv2_route" "post_level" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "POST /levels"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for updating a level.
resource "aws_apigatewayv2_route" "put_level" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "PATCH /levels/{id}"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for uploading a screenshot to s3.
resource "aws_apigatewayv2_route" "put_screenshot" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "POST /screenshot/{filename}"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for fetching all scores for a level.
resource "aws_apigatewayv2_route" "get_scores" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "GET /levels/{id}/scores"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for adding a score for a level.
resource "aws_apigatewayv2_route" "post_score" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "POST /levels/{id}/scores"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for fetching all ratings for a level.
resource "aws_apigatewayv2_route" "get_ratings" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "GET /levels/{id}/ratings"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for adding a rating for a level.
resource "aws_apigatewayv2_route" "post_rating" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "POST /levels/{id}/ratings"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for adding a rating for a level.
resource "aws_apigatewayv2_route" "post_analytics" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "POST /analytics"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for adding a rating for a level.
resource "aws_apigatewayv2_route" "get_user_analytics" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "GET /user/{id}/analytics"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

# Create an api route for adding a rating for a level.
resource "aws_apigatewayv2_route" "get_type_analytics" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "GET /analytics/{type}"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

resource "aws_apigatewayv2_route" "get_all_levels" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "GET /all/levels"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

resource "aws_apigatewayv2_route" "get_all_scores" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "GET /all/scores"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

resource "aws_apigatewayv2_route" "get_all_ratings" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "GET /all/ratings"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

resource "aws_apigatewayv2_route" "get_all_analytics" {
  api_id = aws_apigatewayv2_api.main.id

  route_key = "GET /all/analytics"
  target    = "integrations/${aws_apigatewayv2_integration.editarrr_lambda_integration.id}"
}

resource "aws_lambda_permission" "api_gateway_permission" {
  statement_id  = "AllowExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.editarrr_lambda.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_apigatewayv2_api.main.execution_arn}/*/*"
}

output "dev_base_url" {
  value = aws_apigatewayv2_stage.dev.invoke_url
}
