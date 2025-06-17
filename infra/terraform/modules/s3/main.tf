resource "aws_s3_bucket" "s3_dispute_images_bucket" {
  bucket = "s3-local-bucket"
}

resource "aws_s3_bucket_lifecycle_configuration" "expire_old_files" {
  bucket = aws_s3_bucket.s3_dispute_images_bucket.id

  rule {
    id     = "expires-after-1-day"
    status = "Enabled"

    expiration {
      days = 1
    }
  }
}