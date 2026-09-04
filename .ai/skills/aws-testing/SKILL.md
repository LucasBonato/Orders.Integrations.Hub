**When:** Testing AWS S3 storage, SNS publishing, and SQS delivery.

## Patterns

- Use `FakeStorage` for unit tests of `IObjectStorageClient`.
- Use the shared LocalStack container when testing AWS SDK wiring or real object/message delivery.
- Create AWS clients through `LocalStackAwsClients` with the runtime endpoint from `TestInfrastructure.Environment`.
- Use temporary SQS queues to observe SNS messages and long-poll with `ReceiveMessageAsync`.
- Reset shared S3 objects centrally through `TestInfrastructure`.

## Examples

| Test class | File path | What it tests |
|---|---|---|
| `IFoodDisputeEvidenceStorageTests` | `Test/.../IntegrationTests/Infrastructure/Aws/IFoodDisputeEvidenceStorageTests.cs` | evidence upload to LocalStack S3 |
| `PubSubCommandHandlerTests` | `Test/.../IntegrationTests/Entrypoints/Messaging/PubSubCommandHandlerTests.cs` | SNS delivery observed through SQS |

## Anti-Patterns

- Never call live AWS endpoints or use live credentials in tests.
- Do not use LocalStack when a unit-level fake fully verifies the behavior.
- Do not use sleeps for SNS delivery; use SQS long-polling.
