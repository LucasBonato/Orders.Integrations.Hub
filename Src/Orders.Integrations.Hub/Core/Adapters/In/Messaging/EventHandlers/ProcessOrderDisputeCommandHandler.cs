using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class ProcessOrderDisputeCommandHandler(
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<FastEndpointsCommandEnvelope<ProcessOrderDisputeCommand>> {
    public async Task HandleAsync(FastEndpointsCommandEnvelope<ProcessOrderDisputeCommand> envelope, CancellationToken ct)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var services = scope.Resolve<IOrderDisputeUpdateUseCase>();
        await services.ProcessDispute(envelope.command);
    }
}