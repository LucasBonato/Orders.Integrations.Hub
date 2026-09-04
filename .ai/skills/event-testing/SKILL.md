**When:** Testing MassTransit consumers, command flows, SNS delivery, and external effects.

## Patterns

- Use the real RabbitMQ host through `AppFactory.Create(TestInfrastructure.Environment)`.
- Publish commands through `IPublishEndpoint`.
- Wait for WireMock requests or SQS messages; never sleep.
- Use `TestContext.Current.CancellationToken` for all asynchronous operations.
- Use builders from `TestCommon/Fakers`.

## Examples

| Test class | File path | What it tests |
|---|---|---|
| `CreateOrderCommandHandlerTests` | `Test/.../IntegrationTests/Entrypoints/Messaging/CreateOrderCommandHandlerTests.cs` | consumer to Orders create call |
| `UpdateOrderStatusCommandHandlerTests` | `Test/.../IntegrationTests/Entrypoints/Messaging/UpdateOrderStatusCommandHandlerTests.cs` | consumer to Orders patch call |
| `ProcessOrderDisputeCommandHandlerTests` | `Test/.../IntegrationTests/Entrypoints/Messaging/ProcessOrderDisputeCommandHandlerTests.cs` | consumer to dispute patch call |
| `PubSubCommandHandlerTests` | `Test/.../IntegrationTests/Entrypoints/Messaging/PubSubCommandHandlerTests.cs` | SNS publish observed through SQS |

## Assertions

- Wait for the external effect before asserting its body or count.
- Parse SNS envelopes before asserting the serialized payload.
- Keep one behavior per test.
- Reset shared infrastructure centrally through `TestInfrastructure`.

## Anti-Patterns

- Do not mock `IBus` or `IPublishEndpoint` in transport integration tests.
- Do not call consumers directly when testing broker wiring.
- Do not use `Task.Delay` or process environment mutation for test coordination.
