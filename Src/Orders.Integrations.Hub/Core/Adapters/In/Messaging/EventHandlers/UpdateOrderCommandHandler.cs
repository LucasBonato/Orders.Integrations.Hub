using FastEndpoints;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public class UpdateOrderCommandHandler(
    ILogger<UpdateOrderCommandHandler> logger,
    IOrderClient orderClient
) : IEventHandler<FastEndpointsCommandEnvelope<UpdateOrderStatusCommand>> {
    public async Task HandleAsync(FastEndpointsCommandEnvelope<UpdateOrderStatusCommand> envelope, CancellationToken cancellationToken)
    {
        UpdateOrderStatusCommand command = envelope.Command;

        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("[INFO] - UpdateOrderCommandHandler - Updating Order From: {salesChannel}", command.SalesChannel);
        }
        await orderClient.PatchOrder(command.OrderUpdate);
    }
}