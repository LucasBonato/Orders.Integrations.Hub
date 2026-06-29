terraform {
  required_version = ">= 1.0.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.0"
    }
  }
}

module "s3" {
  source = "../../modules/s3"
}

module "sns" {
  source = "../../modules/sns"
}

output "s3_bucket_name" {
  description = "Name of the S3 bucket for dispute evidence"
  value       = module.s3.s3_bucket_name
}

output "sns_topic_arn" {
  description = "ARN of the accept-order SNS topic"
  value       = module.sns.sns_topic_arn
}
