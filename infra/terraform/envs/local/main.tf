terraform {
  required_version = ">= 1.0.0"

  required_providers {
    aws = {
      source = "hashicorp/aws"
      version = "~> 4.0"
    }
  }
}

resource "aws_instance" "aws_example" {
  tags = {
    Name = "ExampleInstance"
  }
}