variable "aws_secret_key" {
  type        = string
  default     = "dummy"
  description = "Your aws secret key"
}

variable "aws_access_key" {
  type        = string
  default     = "dummy"
  description = "Your aws access key"
}

variable "aws_region" {
  type        = string
  default     = "us-east-1"
  description = "Your main AWS Region"
}

variable "aws_account_id" {
  type        = string
  default     = "123456789012"
  description = "Your account id"
}

variable "aws_base_url" {
  type        = string
  default     = "http://localhost:4566"
  description = "Base URL"
}

variable "instance_type" {
  type        = string
  default     = "t2.micro"
  description = "The type of EC2 instance"
}

variable "aws_skip_key_validations" {
  description = "Skip key validations"
  type        = bool
  default     = true
}