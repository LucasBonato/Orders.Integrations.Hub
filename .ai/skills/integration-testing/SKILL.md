**When:** Testing HTTP pipeline behavior, real message-bus integration, or infrastructure-backed flows.

## Patterns

- **`WebApplicationFactory`** for all application-hosted tests
- **WireMock** for Orders and integration HTTP boundaries
- **`TestInfrastructure`** for shared RabbitMQ, Redis, and LocalStack containers
- **Publish through `IPublishEndpoint`** and wait for the resulting WireMock or SQS observation
- **Mock only external boundaries** in unit tests; use real infrastructure where transport or SDK wiring is under test

## Examples

| Test class | File path | What it tests |
|---|---|---|
| `CreateOrderCommandHandlerTests` | `Test/.../IntegrationTests/Entrypoints/Messaging/CreateOrderCommandHandlerTests.cs` | RabbitMQ consumer posts to Orders |
| `UpdateOrderStatusCommandHandlerTests` | `Test/.../IntegrationTests/Entrypoints/Messaging/UpdateOrderStatusCommandHandlerTests.cs` | RabbitMQ consumer patches Orders |
| `ProcessOrderDisputeCommandHandlerTests` | `Test/.../IntegrationTests/Entrypoints/Messaging/ProcessOrderDisputeCommandHandlerTests.cs` | RabbitMQ consumer patches a dispute |
| `PubSubCommandHandlerTests` | `Test/.../IntegrationTests/Entrypoints/Messaging/PubSubCommandHandlerTests.cs` | SNS delivery verified through SQS |

## Conventions

- **xUnit v3:** `OutputType=Exe` in `.csproj`, `TestContext.Current.CancellationToken` instead of `CancellationToken.None`
- **Test classes are sealed** and real-infrastructure tests derive from `RealInfrastructureTestBase`
- **Use `TestCommon/Fakers`** for command and domain data
- **Use WireMock wait helpers and SQS long-polling** instead of sleeps
- **Keep container setup in `TestInfrastructure`** and external request setup in WireMock helpers

## Anti-Patterns

- Do not use an in-memory message harness for tests whose purpose is to verify RabbitMQ wiring.
- Do not test broker flows by calling handlers or use cases directly.
- Do not use `Task.Delay` for coordination.
- Do not write process environment variables from tests; inject runtime host settings instead.
