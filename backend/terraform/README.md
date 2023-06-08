# Terraform

Terraform is a tool for managing server infrastructure.

### Prerequisites

- [Terraform](https://www.terraform.io/downloads.html) >= 0.12.0
- [AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-install.html) >= 1.16.0

### Usage

- `cd` into the `terraform` directory
- Run `terraform init` to initialize the Terraform configuration
- Run `terraform apply` to apply the Terraform configuration
- Run `terraform destroy` to destroy the Terraform configuration
- Run `terraform plan` to see what changes will be applied
- Run `terraform show` to see the current state
- Run `terraform output` to see the output variables

### Authentication

Terraform uses the AWS CLI to authenticate with AWS. The AWS CLI must be configured with a profile that has the necessary permissions to create the resources defined in the Terraform configuration.

- Run `aws configure sso --profile <profile-name>` to configure the AWS CLI with SSO
- Start url is `https://d-c36717c02f.awsapps.com/start`
- Region is `eu-north-1`
- Run `export AWS_PROFILE=<profile-name>` to set the profile for the current shell session

### Resources

- [Terraform](https://www.terraform.io/)
- [Terraform AWS Provider](https://www.terraform.io/docs/providers/aws/index.html)
