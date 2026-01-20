using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;

using ICommand = Orders.Integrations.Hub.Core.Application.Commands.ICommand;

namespace Orders.Integrations.Hub.Core.Infrastructure.Messaging;

public sealed class FastEndpointsCommandDispatcher : ICommandDispatcher {
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken ct = default) where TCommand : ICommand
    {
        await new FastEndpointsCommandEnvelope<TCommand>(command).PublishAsync(cancellation: ct);
    }
}