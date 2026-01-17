using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class UpdateOrderEventHandler(
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<UpdateOrderStatusCommand> {
    public async Task HandleAsync(UpdateOrderStatusCommand orderCommand, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var service = scope.Resolve<IOrderUseCase>();
        await service.UpdateOrderStatus(orderCommand);
    }
}