using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Core;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Core.Application.EventHandlers;

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