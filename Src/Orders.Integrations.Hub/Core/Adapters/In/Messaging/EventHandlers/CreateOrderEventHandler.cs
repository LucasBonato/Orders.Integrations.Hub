using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

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