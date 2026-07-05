# MassTransit Tests

MassTransit is the backbone of the event-driven architecture. Tests validate consumer behavior, command publishing, and transport configuration.

## MassTransitTestHarnessExtensions

The shared extension method `AddDefaultTestHarness<TConsumer>()` reduces boilerplate:

```csharp
internal static class MassTransitTestHarnessExtensions
{
    public static IServiceCollection AddDefaultTestHarness<TConsumer>(
        this IServiceCollection services
    ) where TConsumer : class, IConsumer =>
        services.AddMassTransitTestHarness(cfg => {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.AddConsumer<TConsumer>();
            cfg.UsingInMemory((context, configurator) => {
                configurator.ConfigureJsonSerializerOptions(options => {
                    options.Converters.Add(new IntegrationKeyJsonConverter());
                    return options;
                });
                configurator.ConfigureEndpoints(context);
            });
        });
}
```

Always use this extension for handler tests. It ensures in-memory transport, kebab-case endpoints, and `IntegrationKeyJsonConverter`.

## In-Memory Bus for Handler Tests

All consumer tests use the in-memory bus via `AddMassTransitTestHarness`:

```csharp
ServiceProvider provider = new ServiceCollection()
    .AddSingleton(_orderClient)
    .AddLogging()
    .AddDefaultTestHarness<CreateOrderCommandHandler>()
    .BuildServiceProvider(true);

_harness = provider.GetRequiredService<ITestHarness>();
await _harness.Start();
await _harness.Bus.Publish(command);
```

## RabbitMQ TestContainers for Transport Tests

Use `TestContainers.RabbitMQ` when testing retry, delayed delivery, or circuit breaker behavior:

```csharp
[Collection("RabbitMQ")]
public class TransportTests : IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitMq;
    private IBusControl _bus;

    public async Task InitializeAsync()
    {
        _rabbitMq = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management-alpine")
            .Build();
        await _rabbitMq.StartAsync();

        _bus = Bus.Factory.CreateUsingRabbitMq(cfg => {
            cfg.Host(_rabbitMq.GetConnectionString());
            cfg.ReceiveEndpoint("test-queue", e => {
                e.Consumer<TestConsumer>();
            });
        });
        await _bus.StartAsync();
    }
}
```

Use sparingly — the in-memory harness is sufficient for most handler tests.

## Retry and Circuit Breaker Testing

To verify retry behavior:

```csharp
services.AddMassTransitTestHarness(cfg => {
    cfg.AddConsumer<TestRetryConsumer>()
        .Endpoint(e => e.Name = "retry-queue");
    cfg.UsingInMemory((context, configurator) => {
        configurator.UseMessageRetry(r => r.Immediate(3));
        configurator.ConfigureEndpoints(context);
    });
});

Assert.Equal(4, consumer.InvocationCount); // 1 initial + 3 retries
```

Circuit breaker testing follows the same pattern — configure `UseCircuitBreaker` and assert messages go to the fault queue after the circuit opens.

## Key Assertions

| Assertion | What It Verifies |
|---|---|
| `harness.Consumed.Any<T>()` | Message of type T consumed by any handler |
| `harness.Published.Any<T>()` | Message of type T published to bus |
| `harness.Published.Any<Fault<T>>()` | Fault published (handler threw) |
| `consumerHarness.Consumed.Select().Count()` | Count consumed by specific handler |
| `harness.Sent.Any<T>()` | Message sent to an endpoint |
