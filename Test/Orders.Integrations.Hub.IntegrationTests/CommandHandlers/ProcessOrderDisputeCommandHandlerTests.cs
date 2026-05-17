using MassTransit;
using MassTransit.Testing;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;
using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.IntegrationTests.CommandHandlers.Extensions;
using Orders.Integrations.Hub.TestCommon.Fakers.Commands;

namespace Orders.Integrations.Hub.IntegrationTests.CommandHandlers;

public sealed class ProcessOrderDisputeCommandHandlerTests : IAsyncLifetime {
    private readonly ITestHarness _harness;
    private readonly IOrderClient _orderClient;

    public ProcessOrderDisputeCommandHandlerTests() {
        _orderClient = Substitute.For<IOrderClient>();

        ServiceProvider provider = new ServiceCollection()
            .AddSingleton(_orderClient)
            .AddDefaultTestHarness<ProcessOrderDisputeCommandHandler>()
            .BuildServiceProvider(true);

        _harness = provider.GetRequiredService<ITestHarness>();
    }

    public async ValueTask InitializeAsync() => await _harness.Start();
    public async ValueTask DisposeAsync() => await _harness.Stop();

    [Fact]
    public async Task Consume_Should_Call_PatchOrderDispute_With_Correct_OrderUpdate() {
        ProcessOrderDisputeCommand? command = new ProcessOrderDisputeCommandFaker()
            .WithType(OrderEventType.DISPUTE_STARTED)
            .WithDispute()
            .Generate();

        await _harness.Bus.Publish(command, TestContext.Current.CancellationToken);

        Assert.True(await _harness.GetConsumerHarness<ProcessOrderDisputeCommandHandler>()
            .Consumed.Any<ProcessOrderDisputeCommand>(TestContext.Current.CancellationToken));

        await _orderClient.Received(1).PatchOrderDispute(Arg.Is<OrderUpdate>(order =>
            order.OrderId == command.ExternalOrderId &&
            order.SourceAppId == command.Integration &&
            order.Type == command.Type &&
            order.FromIntegration == !command.Integration.Equals(IntegrationKey.Nothing()) &&
            order.Dispute != null &&
            order.Dispute.DisputeId == command.OrderDispute!.DisputeId &&
            order.Dispute.Action == command.OrderDispute.Action &&
            order.Dispute.Message == command.OrderDispute.Message
        ));
    }

    [Fact]
    public async Task Consume_Should_Fault_When_Client_Throws() {
        _orderClient
            .PatchOrderDispute(Arg.Any<OrderUpdate>())
            .ThrowsAsync(new Exception("client error"));

        ProcessOrderDisputeCommand command = new ProcessOrderDisputeCommandFaker()
            .WithType(OrderEventType.DISPUTE_STARTED)
            .Generate();
        
        await _harness.Bus.Publish(command, TestContext.Current.CancellationToken);

        Assert.True(await _harness.Published.Any<Fault<ProcessOrderDisputeCommand>>(TestContext.Current.CancellationToken));
    }
}