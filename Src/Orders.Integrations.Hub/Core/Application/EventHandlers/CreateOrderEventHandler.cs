using FastEndpoints;

using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Core;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Core.Application.EventHandlers;

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