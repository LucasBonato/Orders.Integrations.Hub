using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Events;

using FastEndpoints;

namespace BizPik.Orders.Hub.Core.Orders.Application.EventHandlers;

public class CreateOrderEventHandler(
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<CreateOrderEvent> {
    public async Task HandleAsync(CreateOrderEvent orderEvent, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var services = scope.Resolve<IOrderUseCase>();
        await services.CreateOrder(orderEvent);
    }
}