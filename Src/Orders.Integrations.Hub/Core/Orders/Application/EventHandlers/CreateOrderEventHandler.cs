using Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;

using FastEndpoints;

namespace Orders.Integrations.Hub.Core.Orders.Application.EventHandlers;

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