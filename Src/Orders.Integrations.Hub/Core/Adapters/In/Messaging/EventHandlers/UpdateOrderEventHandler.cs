using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

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