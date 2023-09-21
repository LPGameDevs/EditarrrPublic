https://andmoredev.medium.com/how-to-run-api-gateway-aws-lambda-and-dynamodb-locally-91b75d9a54fe


### Setup
- `docker network create aws-local-api-network`
- `docker run -d -p 8000:8000 --network=aws-local-api-network --name dynamo-local amazon/dynamodb-local`

### Testing
- `aws dynamodb list-tables --endpoint-url http://localhost:8000`


**Response**:

```json
{
    "TableNames": []
}
```

- `sam local start-api --docker-network aws-local-api-network`


### SAM - INIT project

- `sam init`
- node-18 / image

```
Commands you can use next
=========================
[*] Create pipeline: cd sam-hello-node18-image && sam pipeline init --bootstrap
[*] Validate SAM template: cd sam-hello-node18-image && sam validate
[*] Test Function in the Cloud: cd sam-hello-node18-image && sam sync --stack-name {stack-name} --watch

```


**CI setup**

This is complicated :(

- `sam pipeline init --bootstrap`