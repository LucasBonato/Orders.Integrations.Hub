using MassTransit;
using MassTransit.Testing;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;
using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.IntegrationTests.CommandHandlers.Extensions;
using Orders.Integrations.Hub.TestCommon.Fakers.Commands;

namespace Orders.Integrations.Hub.IntegrationTests.CommandHandlers;

public sealed class UpdateOrderCommandHandlerTests : IAsyncLifetime {
    private readonly ITestHarness _harness;
    private readonly IOrderClient _orderClient;

    public UpdateOrderCommandHandlerTests() {
        _orderClient = Substitute.For<IOrderClient>();

        ServiceProvider provider = new ServiceCollection()
            .AddSingleton(_orderClient)
            .AddLogging()
            .AddDefaultTestHarness<UpdateOrderCommandHandler>()
            .BuildServiceProvider(true);

        _harness = provider.GetRequiredService<ITestHarness>();
    }

    public async ValueTask InitializeAsync() => await _harness.Start();
    public async ValueTask DisposeAsync() => await _harness.Stop();

    [Fact]
    public async Task Consume_Should_Call_OrderClient_PatchOrder() {
        UpdateOrderStatusCommand command = new UpdateOrderStatusCommandFaker().Generate();

        await _harness.Bus.Publish(command, TestContext.Current.CancellationToken);

        Assert.True(await _harness.GetConsumerHarness<UpdateOrderCommandHandler>()
            .Consumed.Any<UpdateOrderStatusCommand>(TestContext.Current.CancellationToken));

        await _orderClient.Received(1).PatchOrder(command.OrderUpdate);
    }

    [Fact]
    public async Task Consume_Should_Fault_When_Client_Throws() {
        _orderClient
            .PatchOrder(Arg.Any<OrderUpdate>())
            .ThrowsAsync(new Exception("client error"));

        await _harness.Bus.Publish(new UpdateOrderStatusCommandFaker().Generate(), TestContext.Current.CancellationToken);

        Assert.True(await _harness.Published.Any<Fault<UpdateOrderStatusCommand>>(TestContext.Current.CancellationToken));
    }
}