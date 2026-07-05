# aws-testing

**When:** Testing AWS S3 storage, SNS messaging, or any AWS service integration.

## Patterns

- **`FakeStorage`** (`TestCommon/Utilities/FakeStorage.cs`) — in-memory `IObjectStorageClient` implementation for unit tests without AWS SDK
- **NSubstitute mocks** for `IAmazonSimpleNotificationService` in MassTransit handler tests (see `PubSubCommandHandlerTests`)
- **`FakeCache`** (`TestCommon/Utilities/FakeCache.cs`) — in-memory `ICacheService` for testing caching behavior without Redis
- **In-memory replacements** for AWS services — never call real AWS endpoints in tests

## Examples

| Pattern | File Path | What It Tests |
|---|---|---|
| `FakeStorage` | `TestCommon/Utilities/FakeStorage.cs` | in-memory `IObjectStorageClient` — `UploadFile`, `DeleteFile`, `GetTemporaryUrl` |
| `PubSubCommandHandlerTests` | `Test/.../IntegrationTests/CommandHandlers/PubSubCommandHandlerTests.cs` | SNS publish via mock, fault propagation |
| `FakeCache` | `TestCommon/Utilities/FakeCache.cs` | in-memory `ICacheService` with TTL support |

## Conventions

- **`FakeStorage`** (in `TestCommon/Utilities/`): thread-safe `ConcurrentDictionary` backing; tracks file count for URL uniqueness; implements `UploadFile`, `DeleteFile`, `DeleteFolder`, `GetTemporaryUrl`, `Exists`
- **`FakeCache`** (in `TestCommon/Utilities/`): thread-safe `ConcurrentDictionary` backing; TTL-aware `GetAsync` returns `default` after expiry
- **Mock SNS** with `Substitute.For<IAmazonSimpleNotificationService>()` + `.PublishAsync(...).Returns(new PublishResponse { MessageId = ... })`
- **Test both success and fault** paths for every AWS interaction
- **Never use `LocalStack` or `TestContainers`** for AWS unless the behavior cannot be replicated in-memory (consult team first)

## Anti-Patterns

- ❌ **Calling real AWS services** in any test — never use live credentials
- ❌ **Using `LocalStack` when `FakeStorage` suffices** — in-memory fakes are faster and don't require container orchestration
- ❌ **Testing SNS topic ARN parsing in the handler test** — that's a unit test; handler tests verify the mock was called with correct ARN
- ❌ **Over-mocking** — if the interface is simple (e.g., `IObjectStorageClient`), prefer `FakeStorage` over `Substitute.For<>()`
