# Project dependencies
terraform {
  backend "s3" {
    bucket                  = "editarrrr-terraform-state-bucket"
    profile                 = "editarrr-dev"
    key                     = "terraform_state/terraform.tfstate"
    region                  = "eu-north-1"
  }
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.0"
    }
    random = {
      # Generates a random string for the S3 bucket
      source  = "hashicorp/random"
      version = "~> 3.3.0"
    }
    archive = {
      source  = "hashicorp/archive"
      version = "~> 2.3.0"
    }
  }
}

# Configure the AWS Provider
provider "aws" {
  region  = "eu-north-1"
  profile = "editarrr-dev"
}
