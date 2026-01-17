using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class CreateOrderCommandHandler(
    IServiceScopeFactory serviceScopeFactory
) : IEventHandler<FastEndpointsCommandEnvelope<CreateOrderCommand>> {
    public async Task HandleAsync(FastEndpointsCommandEnvelope<CreateOrderCommand> envelope, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var services = scope.Resolve<IOrderUseCase>();
        await services.CreateOrder(envelope.command);
    }
}