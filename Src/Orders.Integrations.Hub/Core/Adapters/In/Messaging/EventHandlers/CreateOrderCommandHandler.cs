using System.Text.Json;

using MassTransit;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;

public sealed class CreateOrderCommandHandler(
    ILogger<CreateOrderCommandHandler> logger,
    IOrderClient orderClient
) : IConsumer<CreateOrderCommand> {
    public async Task Consume(ConsumeContext<CreateOrderCommand> context) {
        CreateOrderCommand command = context.Message;

        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("[INFO] - CreateOrderCommandHandler - Creating Order From {salesChannel}", command.Order.SalesChannel);
            logger.LogInformation("[INFO] - CreateOrderCommandHandler - Order: {order}", JsonSerializer.Serialize(command.Order));
        }
        await orderClient.CreateOrder(command.Order);
    }
}