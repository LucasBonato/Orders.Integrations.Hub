# Event / Command Handler Tests

Command handlers are MassTransit consumers that receive commands from the bus and orchestrate use cases. Tests use the `ITestHarness` with in-memory transport.

## Testing Consumers with ITestHarness

All command handler tests follow the same pattern (see `CreateOrderCommandHandlerTests`):

```csharp
public sealed class CreateOrderCommandHandlerTests : IAsyncLifetime
{
    private readonly ITestHarness _harness;
    private readonly IOrderClient _orderClient;

    public CreateOrderCommandHandlerTests()
    {
        _orderClient = Substitute.For<IOrderClient>();
        ServiceProvider provider = new ServiceCollection()
            .AddSingleton(_orderClient)
            .AddLogging()
            .AddDefaultTestHarness<CreateOrderCommandHandler>()
            .BuildServiceProvider(true);
        _harness = provider.GetRequiredService<ITestHarness>();
    }

    public async ValueTask InitializeAsync() => await _harness.Start();
    public async ValueTask DisposeAsync() => await _harness.Stop();
}
```

`AddDefaultTestHarness<TConsumer>()` configures in-memory transport, kebab-case naming, `IntegrationKeyJsonConverter`, and consumer registration.

## Three Essential Scenarios

### 1. Success — command consumed, mock interaction verified

```csharp
[Fact]
public async Task Consume_Should_Call_OrderClient_CreateOrder()
{
    CreateOrderCommand command = new CreateOrderCommandFaker().Generate();
    await _harness.Bus.Publish(command);

    Assert.True(await _harness.GetConsumerHarness<CreateOrderCommandHandler>()
        .Consumed.Any<CreateOrderCommand>());

    await _orderClient.Received(1).CreateOrder(Arg.Is<Order>(o =>
        o.OrderId == command.Order.OrderId));
}
```

### 2. Fault on failure — asserts Fault<T> published

```csharp
[Fact]
public async Task Consume_Should_Fault_When_Client_Throws()
{
    _orderClient.CreateOrder(Arg.Any<Order>())
        .ThrowsAsync(new Exception("client error"));
    await _harness.Bus.Publish(new CreateOrderCommandFaker().Generate());

    Assert.True(await _harness.Published.Any<Fault<CreateOrderCommand>>());
}
```

### 3. No fault on success

```csharp
[Fact]
public async Task Consume_Should_Not_Fault_When_Client_Succeeds()
{
    await _harness.Bus.Publish(new CreateOrderCommandFaker().Generate());
    Assert.False(await _harness.Published.Any<Fault<CreateOrderCommand>>());
}
```

## Testing Command Publishing

```csharp
[Fact]
public async Task DispatchAsync_Should_Publish_Command_To_Bus()
{
    CreateOrderCommand command = new CreateOrderCommandFaker().Generate();
    await _dispatcher.DispatchAsync(command);
    Assert.True(await _harness.Published.Any<CreateOrderCommand>());
}
```

## Key Assertions

- `harness.Consumed.Any<T>()` — message consumed by any handler
- `harness.Published.Any<T>()` — message published to bus
- `harness.Published.Any<Fault<T>>()` — fault was published
- `harness.GetConsumerHarness<TConsumer>().Consumed.Any<T>()` — specific handler consumed
- `Received(1)` / `DidNotReceiveWithAnyArgs()` — precise mock interaction verification
