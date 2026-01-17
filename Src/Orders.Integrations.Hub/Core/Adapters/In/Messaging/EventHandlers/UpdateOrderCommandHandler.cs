using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class UpdateOrderCommandHandler(
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<FastEndpointsCommandEnvelope<UpdateOrderStatusCommand>> {
    public async Task HandleAsync(FastEndpointsCommandEnvelope<UpdateOrderStatusCommand> envelope, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var service = scope.Resolve<IOrderUseCase>();
        await service.UpdateOrderStatus(envelope.command);
    }
}