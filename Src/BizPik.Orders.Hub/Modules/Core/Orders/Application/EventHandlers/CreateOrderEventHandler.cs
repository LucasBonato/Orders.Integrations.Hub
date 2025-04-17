using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.EventHandlers;

public class CreateOrderEventHandler(
    ILogger<CreateOrderEventHandler> logger,
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<CreateOrderEvent>, IOrderHttp {
    public async Task HandleAsync(CreateOrderEvent orderEvent, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var services = scope.Resolve<IOrderUseCase>();
        await services.CreateOrder(orderEvent);
    }
}