using System.Text.Json;

using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class CreateOrderCommandHandler(
    ILogger<CreateOrderCommandHandler> logger,
    IOrderClient orderClient
) : IEventHandler<FastEndpointsCommandEnvelope<CreateOrderCommand>> {
    public async Task HandleAsync(FastEndpointsCommandEnvelope<CreateOrderCommand> envelope, CancellationToken cancellationToken)
    {
        CreateOrderCommand command = envelope.Command;

        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("[INFO] - CreateOrderCommandHandler - Creating Order From {salesChannel}", command.Order.SalesChannel);
            logger.LogInformation("[INFO] - CreateOrderCommandHandler - Order: {order}", JsonSerializer.Serialize(command.Order));
        }
        await orderClient.CreateOrder(command.Order);
    }
}