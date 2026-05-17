using MassTransit;
using MassTransit.Testing;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;
using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.IntegrationTests.CommandHandlers.Extensions;
using Orders.Integrations.Hub.TestCommon.Fakers.Commands;

namespace Orders.Integrations.Hub.IntegrationTests.CommandHandlers;

public sealed class CreateOrderCommandHandlerTests : IAsyncLifetime {
    private readonly ITestHarness _harness;
    private readonly IOrderClient _orderClient;

    public CreateOrderCommandHandlerTests() {
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

    [Fact]
    public async Task Consume_Should_Call_OrderClient_CreateOrder() {
        CreateOrderCommand command = new CreateOrderCommandFaker().Generate();

        await _harness.Bus.Publish(command, TestContext.Current.CancellationToken);

        Assert.True(await _harness.GetConsumerHarness<CreateOrderCommandHandler>()
            .Consumed.Any<CreateOrderCommand>(TestContext.Current.CancellationToken));
        
        await _orderClient.Received(1).CreateOrder(
            Arg.Is<Order>(order =>
                order.OrderId == command.Order.OrderId &&
                order.TenantId == command.Order.TenantId &&
                order.SalesChannel == command.Order.SalesChannel &&
                order.Type == command.Order.Type
            )
        );
    }

    [Fact]
    public async Task Consume_Should_Not_Fault_When_Client_Succeeds() {
        CreateOrderCommand command = new CreateOrderCommandFaker().Generate();

        await _harness.Bus.Publish(command, TestContext.Current.CancellationToken);

        Assert.False(await _harness.Published.Any<Fault<CreateOrderCommand>>(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Consume_Should_Fault_When_Client_Throws() {
        _orderClient
            .CreateOrder(Arg.Any<Order>())
            .ThrowsAsync(new Exception("client error"));
        
        CreateOrderCommand command = new CreateOrderCommandFaker().Generate();

        await _harness.Bus.Publish(command, TestContext.Current.CancellationToken);

        Assert.True(await _harness.Published.Any<Fault<CreateOrderCommand>>(TestContext.Current.CancellationToken));
    }
}