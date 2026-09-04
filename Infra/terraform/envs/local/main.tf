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

output "dispute_images_bucket_name" {
  description = "Name of the S3 bucket used for dispute evidence"
  value       = module.s3.s3_dispute_images_bucket_name
}

output "accept_order_topic_arn" {
  description = "ARN of the accept-order SNS topic"
  value       = module.sns.sns_topic_arn
}