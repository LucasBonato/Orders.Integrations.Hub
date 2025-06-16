using BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Events;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Application.EventHandlers;

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