# event-testing

**When:** Testing MassTransit consumers, command handlers, event flows, pub/sub, and fault handling.

## Patterns

- **MassTransit `ITestHarness`** for in-memory bus simulation — `Start()`/`Stop()` via `IAsyncLifetime`
- **`IConsumer<T>` testing** — publish command via `_harness.Bus.Publish()`, verify with `_harness.Consumed.Any<T>()`
- **Fault testing** — verify `_harness.Published.Any<Fault<T>>()` when mocks throw
- **Mock external dependencies** (clients, SNS, cache) with NSubstitute, inject into DI
- **`AddDefaultTestHarness<TConsumer>()`** extension from `MassTransitTestHarnessExtensions.cs` — standardizes in-memory config with `IntegrationKeyJsonConverter`

## Examples

| Test Class | File Path | What It Tests |
|---|---|---|
| `CreateOrderCommandHandlerTests` | `Test/.../IntegrationTests/CommandHandlers/CreateOrderCommandHandlerTests.cs` | consumer → `IOrderClient`, fault on error |
| `UpdateOrderCommandHandlerTests` | `Test/.../IntegrationTests/CommandHandlers/UpdateOrderCommandHandlerTests.cs` | consumer → `IOrderClient.PatchOrder`, fault |
| `ProcessOrderDisputeCommandHandlerTests` | `Test/.../IntegrationTests/CommandHandlers/ProcessOrderDisputeCommandHandlerTests.cs` | consumer → `PatchOrderDispute` with arg matching |
| `PubSubCommandHandlerTests` | `Test/.../IntegrationTests/CommandHandlers/PubSubCommandHandlerTests.cs` | consumer → `IAmazonSNS`, fault on SNS error |
| `CommandDispatcherTests` | `Test/.../IntegrationTests/CommandHandlers/CommandDispatcherTests.cs` | dispatcher → bus publish |

## Conventions

- **Constructor** sets up `ServiceCollection` with mocks + `AddDefaultTestHarness<T>()`, resolves `ITestHarness`
- **`IAsyncLifetime.InitializeAsync`** → `await _harness.Start()`
- **`IAsyncLifetime.DisposeAsync`** → `await _harness.Stop()`
- **Assert consumption** with `GetConsumerHarness<T>().Consumed.Any<T>()` before asserting mock interactions
- **Test both success and fault paths** — each in its own `[Fact]`
- **Use `Fakers`** for command generation: `new CreateOrderCommandFaker().WithOrder(...).Generate()`
- **Use `Arg.Is<>()`** for structured argument matching on mocked dependencies

## Anti-Patterns

- ❌ **Mocking `IBus` or `IPublishEndpoint`:** Use `ITestHarness.Bus` — the harness provides the real in-memory bus
- ❌ **Verifying mock calls before bus delivery:** Always assert `Consumed.Any<T>()` first, then verify mock received calls
- ❌ **Testing handlers in isolation without harness:** Consumer/handler logic includes middleware, serialization, and DI — test through the harness
- ❌ **Missing fault tests:** Every handler with an external dependency should have a fault-publish test
- ❌ **Shared harness across test classes:** Each test class creates its own `ITestHarness`
