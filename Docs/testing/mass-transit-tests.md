## Overview

MassTransit is the application message boundary. Integration tests use the real RabbitMQ transport; unit tests cover consumer-adjacent behavior through the existing unit-test patterns.

## Real Transport Tests

Real consumer tests live under `Test/Orders.Integrations.Hub.IntegrationTests/Entrypoints/Messaging/` and use:

- `AppFactory.Create(testInfrastructure.Environment)` with RabbitMQ enabled.
- `IPublishEndpoint` to publish the command.
- WireMock request waiters for Orders API calls.
- LocalStack SQS long-polling for SNS delivery assertions.
- `TestContext.Current.CancellationToken` for cancellation.

Example:

```csharp
Host.WireMock.OrdersApi.StubCreateOrder();
CreateOrderCommand command = new CreateOrderCommandFaker().Generate();

using IServiceScope scope = Host.Services.CreateScope();
IPublishEndpoint bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
await bus.Publish(command, TestContext.Current.CancellationToken);

await Host.WireMock.OrdersApi.WaitForCreateOrderAsync(TestContext.Current.CancellationToken);
```

## Container Lifecycle

`TestInfrastructure` owns Redis, RabbitMQ, and LocalStack through `IAsyncLifetime`. The shared `IntegrationTestCollection` resets queues, cache, and object storage before each test instance and disposes all containers afterward.

## Assertions

- Wait for the observable external effect instead of sleeping.
- Assert the request path, count, and serialized body at the WireMock boundary.
- For SNS, subscribe a temporary SQS queue and use `ReceiveMessageAsync` long-polling.
- Keep each test focused on one consumer behavior.
