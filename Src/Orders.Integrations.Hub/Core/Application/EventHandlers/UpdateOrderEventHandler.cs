using Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;

using FastEndpoints;

namespace Orders.Integrations.Hub.Core.Orders.Application.EventHandlers;

public class UpdateOrderEventHandler(
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<UpdateOrderStatusEvent> {
    public async Task HandleAsync(UpdateOrderStatusEvent orderEvent, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var service = scope.Resolve<IOrderUseCase>();
        await service.UpdateOrderStatus(orderEvent);
    }
}