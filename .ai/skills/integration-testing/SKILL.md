# integration-testing

**When:** Testing MassTransit handler execution, HTTP pipeline behavior, message bus integration, or infrastructure-backed flows.

## Patterns

- **`ITestHarness`** from `MassTransit.Testing` for in-memory message bus tests — start in `InitializeAsync`, stop in `DisposeAsync`
- **`IAsyncLifetime`** for harness lifecycle: `InitializeAsync` calls `await _harness.Start()`, `DisposeAsync` calls `await _harness.Stop()`
- **`WebApplicationFactory`** + `TestContainers.Redis` for HTTP endpoint integration tests
- **`AddDefaultTestHarness<TConsumer>()`** from `MassTransitTestHarnessExtensions` — sets up kebab-case endpoints, `IntegrationKeyJsonConverter`, in-memory transport
- **Mock only external boundaries** via `Substitute.For<T>()` — inject mocks into `ServiceCollection` with `AddSingleton`
- **Assert consumption** with `_harness.GetConsumerHarness<T>().Consumed.Any<T>()`
- **Assert faults** with `_harness.Published.Any<Fault<T>>()`

## Examples

| Test Class | File Path | What It Tests |
|---|---|---|
| `CreateOrderCommandHandlerTests` | `Test/.../IntegrationTests/CommandHandlers/CreateOrderCommandHandlerTests.cs` | consumer calls client, fault on error |
| `UpdateOrderCommandHandlerTests` | `Test/.../IntegrationTests/CommandHandlers/UpdateOrderCommandHandlerTests.cs` | consumer calls PatchOrder, fault on error |
| `ProcessOrderDisputeCommandHandlerTests` | `Test/.../IntegrationTests/CommandHandlers/ProcessOrderDisputeCommandHandlerTests.cs` | consumer calls PatchOrderDispute, arg matching |
| `PubSubCommandHandlerTests` | `Test/.../IntegrationTests/CommandHandlers/PubSubCommandHandlerTests.cs` | SNS publish via mock, fault on SNS error |
| `CommandDispatcherTests` | `Test/.../IntegrationTests/CommandHandlers/CommandDispatcherTests.cs` | dispatcher publishes command to bus |

## Conventions

- **xUnit v3:** `OutputType=Exe` in `.csproj`, `TestContext.Current.CancellationToken` instead of `CancellationToken.None`
- **Test class is `sealed`** implements `IAsyncLifetime` (see all integration test handler classes)
- **Constructor builds SUT** (harness + mocks), `IAsyncLifetime` methods manage lifecyle
- **Use `TestCommon/Fakers`** for command generation: `new CreateOrderCommandFaker().Generate()`
- **Use `TestCommon/Utilities/FakeCache`** or **`FakeStorage`** as in-memory fakes instead of mocking for simple contracts
- **Keep harness setup in `AddDefaultTestHarness`** extension method to avoid duplication
- **Prefer `[Fact]` over `[Theory]`** for handler tests — one command type per test class

## Anti-Patterns

- ❌ **Mocking the message bus:** Use `ITestHarness` with in-memory transport, never `Substitute.For<IBus>()`
- ❌ **Using real RabbitMQ/Redis** in unit or standard integration tests — use `TestContainers` only when in-memory isn't sufficient (e.g., Redis cluster behavior tests)
- ❌ **Testing command handlers via mediator/service directly instead of through the bus** — publish the command and verify consumption
- ❌ **`Task.Delay` for harness coordination** — use `ITestHarness.Consumed.Any()` which polls efficiently
- ❌ **Multiple consumers per test class** — one handler per test file keeps tests focused
