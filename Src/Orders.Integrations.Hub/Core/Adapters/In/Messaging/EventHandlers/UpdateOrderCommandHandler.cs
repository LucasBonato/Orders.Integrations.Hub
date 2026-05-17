using MassTransit;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public sealed class UpdateOrderCommandHandler(
    ILogger<UpdateOrderCommandHandler> logger,
    IOrderClient orderClient
) : IConsumer<UpdateOrderStatusCommand> {
    public async Task Consume(ConsumeContext<UpdateOrderStatusCommand> context)
    {
        UpdateOrderStatusCommand command = context.Message;

        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("[INFO] - UpdateOrderCommandHandler - Updating Order From: {salesChannel}", command.SalesChannel);
        }
        await orderClient.PatchOrder(command.OrderUpdate);
    }
}