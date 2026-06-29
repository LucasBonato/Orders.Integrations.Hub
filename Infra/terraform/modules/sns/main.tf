resource "aws_sns_topic" "accept_order" {
  name = "accept-order"
}

output "sns_topic_arn" {
  description = "ARN of the accept-order SNS topic"
  value       = aws_sns_topic.accept_order.arn
}
