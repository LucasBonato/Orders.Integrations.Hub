using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Orders.Application.EventHandlers;

public class PatchOrderDisputeEventHandler(
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<ProcessOrderDisputeEvent> {
    public async Task HandleAsync(ProcessOrderDisputeEvent orderDisputeEvent, CancellationToken ct)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var services = scope.Resolve<IOrderDisputeUpdateUseCase>();
        await services.ProcessDispute(orderDisputeEvent);
    }
}