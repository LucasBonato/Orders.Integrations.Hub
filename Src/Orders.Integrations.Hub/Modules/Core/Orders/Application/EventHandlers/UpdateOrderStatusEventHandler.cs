using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Application.EventHandlers;

public class UpdateOrderStatusEventHandler(
    ILogger<UpdateOrderStatusEventHandler> logger,
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<UpdateOrderStatusEvent> {
    public async Task HandleAsync(UpdateOrderStatusEvent orderEvent, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var service = scope.Resolve<IOrderUseCase>();
        await service.UpdateOrderStatus(orderEvent);
    }
}