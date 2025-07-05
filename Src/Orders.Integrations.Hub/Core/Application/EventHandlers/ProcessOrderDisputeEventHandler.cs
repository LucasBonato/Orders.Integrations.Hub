using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Core.Application.EventHandlers;

public class ProcessOrderDisputeEventHandler(
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<ProcessOrderDisputeEvent> {
    public async Task HandleAsync(ProcessOrderDisputeEvent orderDisputeEvent, CancellationToken ct)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var services = scope.Resolve<IOrderDisputeUpdateUseCase>();
        await services.ProcessDispute(orderDisputeEvent);
    }
}