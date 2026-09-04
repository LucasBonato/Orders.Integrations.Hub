## Overview

Messaging tests exercise commands through the real RabbitMQ transport and verify the resulting external effect. They are not in-memory MassTransit harness tests.

## Consumer Round Trip

```csharp
Host.WireMock.OrdersApi.StubPatchOrder();
UpdateOrderStatusCommand command = new UpdateOrderStatusCommandFaker().Generate();

using IServiceScope scope = Host.Services.CreateScope();
IPublishEndpoint bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
await bus.Publish(command, TestContext.Current.CancellationToken);

await Host.WireMock.OrdersApi.WaitForPatchOrderAsync(TestContext.Current.CancellationToken);
```

Use the corresponding WireMock waiter for dispute and create-order commands. Assertions should verify one behavior: the expected external call and its serialized data.

## SNS Events

Subscribe a temporary SQS queue to the LocalStack topic, publish the command, then use `ReceiveMessageAsync` with long-polling. Parse the SNS envelope before asserting the serialized message.

## Conventions

- Test classes are sealed and use `IAsyncLifetime` through `RealInfrastructureTestBase`.
- Use builders from `TestCommon/Fakers`.
- Reset shared infrastructure centrally; do not add per-test sleeps or environment-variable races.
- Keep unit tests for pure consumer logic and failure mapping; use real transport tests for broker and endpoint wiring.
