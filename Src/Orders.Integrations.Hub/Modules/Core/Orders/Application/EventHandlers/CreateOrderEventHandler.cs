using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using FastEndpoints;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Application.EventHandlers;

public class CreateOrderEventHandler(
    ILogger<CreateOrderEventHandler> logger,
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<CreateOrderEvent> {
    public async Task HandleAsync(CreateOrderEvent orderEvent, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var services = scope.Resolve<IOrderUseCase>();
        await services.CreateOrder(orderEvent);
    }
}