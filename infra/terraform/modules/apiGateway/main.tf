resource "aws_api_gateway_rest_api" "gateway-sa-east-1" {
  tags = {
    Name = "ExampleInstance" # Tag the instance with a Name tag for easier identification
  }
  name = ""
}