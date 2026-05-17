using MassTransit;
using MassTransit.Testing;

using Microsoft.Extensions.DependencyInjection;

using Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;
using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;
using Orders.Integrations.Hub.TestCommon.Fakers.Commands;

namespace Orders.Integrations.Hub.IntegrationTests.CommandHandlers;

public class CommandDispatcherTests : IAsyncLifetime
{
    private readonly ITestHarness _harness;
    private readonly ICommandDispatcher _dispatcher;

    public CommandDispatcherTests()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg => {
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.AddConsumer<CreateOrderCommandHandler>();
                cfg.AddConsumer<UpdateOrderCommandHandler>();
                cfg.AddConsumer<PubSubCommandHandler>();
                cfg.AddConsumer<ProcessOrderDisputeCommandHandler>();
            })
            .BuildServiceProvider(true);

        _harness = provider.GetRequiredService<ITestHarness>();
        _dispatcher = new MassTransitCommandDispatcher(_harness.Bus);
    }

    public async ValueTask InitializeAsync() => await _harness.Start();

    public async ValueTask DisposeAsync()
    {
        await _harness.Stop();

        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task DispatchAsync_Should_Publish_Command_To_Bus()
    {
        CreateOrderCommand command = new CreateOrderCommandFaker().Generate();

        await _dispatcher.DispatchAsync(command, TestContext.Current.CancellationToken);

        Assert.True(await _harness.Published.Any<CreateOrderCommand>(TestContext.Current.CancellationToken));
    }
}