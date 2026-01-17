using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class ProcessOrderDisputeEventHandler(
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<FastEndpointsCommandEnvelope<ProcessOrderDisputeCommand>> {
    public async Task HandleAsync(ProcessOrderDisputeCommand orderDisputeCommand, CancellationToken ct)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var services = scope.Resolve<IOrderDisputeUpdateUseCase>();
        await services.ProcessDispute(orderDisputeCommand);
    }
}