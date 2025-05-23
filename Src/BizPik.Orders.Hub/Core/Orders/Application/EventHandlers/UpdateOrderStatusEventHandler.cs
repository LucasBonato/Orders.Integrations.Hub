using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Orders.Application.EventHandlers;

public class UpdateOrderStatusEventHandler(
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<UpdateOrderStatusEvent> {
    public async Task HandleAsync(UpdateOrderStatusEvent orderEvent, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var service = scope.Resolve<IOrderUseCase>();
        await service.UpdateOrderStatus(orderEvent);
    }
}