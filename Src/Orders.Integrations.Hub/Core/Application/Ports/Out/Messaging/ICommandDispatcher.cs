using Orders.Integrations.Hub.Core.Application.Commands;

namespace Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;

public interface ICommandDispatcher {
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken ct = default) where TCommand : ICommand;
}