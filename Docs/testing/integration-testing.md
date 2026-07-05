# Integration Testing

Integration tests validate the **real wiring** between components — message bus, HTTP pipeline, cache backends, and storage services. They use `ITestHarness` for MassTransit consumers, `TestContainers` for infrastructure dependencies, and `WebApplicationFactory` for HTTP endpoints.

## MassTransit Test Harness

The `ITestHarness` pattern is used for all command handler tests. This validates that consumers receive messages, interact with mocked dependencies, and publish faults on failure.

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

Use `AddDefaultTestHarness<TConsumer>()` to configure in-memory bus, kebab-case endpoints, and `IntegrationKeyJsonConverter`. Three scenarios per consumer: success, fault on failure, no fault on success.

## TestContainers

TestContainers provide real infrastructure for integration tests:

| Container | Use Case |
|---|---|
| **Redis** | Cache integration tests (real `IDistributedCache`) |
| **LocalStack** | S3 storage tests (dispute evidence) |
| **RabbitMQ** | Transport-level message tests (retry, circuit breaker) |

Each expensive container uses a **collection fixture**:

```csharp
[CollectionDefinition("Redis")]
public class RedisCollection : ICollectionFixture<RedisContainerFixture> { }

[Collection("Redis")]
public class WebhookEndpointTests : IAsyncLifetime { ... }
```

## WebApplicationFactory

For full HTTP pipeline tests (webhook endpoints, signature filters, middleware):

```csharp
public class WebhookEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WebhookEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(Substitute.For<IInternalClient>());
            });
        }).CreateClient();
    }
}
```

## When to Use Each Approach

| Need | Approach |
|---|---|
| Test a MassTransit consumer | `ITestHarness` + mocked dependencies |
| Test full HTTP webhook pipeline | `WebApplicationFactory` + `ITestHarness` |
| Test cache behavior with real Redis | `TestContainers.Redis` collection fixture |
| Test S3 operations | `FakeStorage` for unit, `TestContainers.LocalStack` for integration |
| Test message transport | `TestContainers.RabbitMQ` |
