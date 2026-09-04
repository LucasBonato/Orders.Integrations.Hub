## Overview

`IObjectStorageClient`, implemented by `SimpleStorageServiceClient`, handles dispute evidence in S3. Unit tests use `FakeStorage`; the real-infrastructure integration test uses the shared LocalStack container.

## Unit Tests

Use `FakeStorage` for fast tests of upload, delete, folder deletion, and temporary URL behavior. Keep AWS SDK concerns out of these tests.

## LocalStack Integration Test

`IFoodDisputeEvidenceStorageTests` runs in `IntegrationTestCollection` and uses `LocalStackAwsClients` to verify that webhook dispute evidence is uploaded to the configured bucket.

The shared fixture creates the bucket before tests and removes stored objects during reset. Tests use the runtime endpoint from `TestInfrastructure.Environment`; they do not use live AWS credentials or a separate Terraform stack.

## Coverage

- Upload evidence and verify the stored object.
- Preserve the expected content type and object key.
- Delete objects when the dispute lifecycle requires it.
- Verify temporary URL behavior with `FakeStorage` unit tests.
