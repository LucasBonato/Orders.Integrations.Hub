using FastEndpoints;

using ICommand = Orders.Integrations.Hub.Core.Application.Commands.ICommand;

namespace Orders.Integrations.Hub.Core.Infrastructure.Messaging;

public sealed record FastEndpointsCommandEnvelope<TCommand>(
    TCommand command
) : IEvent where TCommand : ICommand;