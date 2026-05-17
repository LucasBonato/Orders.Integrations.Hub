using MassTransit;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;

namespace Orders.Integrations.Hub.Core.Infrastructure.Messaging;

public sealed class MassTransitCommandDispatcher(
    IBus bus
) : ICommandDispatcher {
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken ct = default) where TCommand : ICommand
    {
        await bus.Publish(command, ct);
    }
}