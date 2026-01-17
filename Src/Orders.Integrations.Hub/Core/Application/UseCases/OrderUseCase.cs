using System.Text.Json;

using Orders.Integrations.Hub.Core.Application.Commands;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.Core.Application.UseCases;

public class OrderUseCase(
    ILogger<OrderUseCase> logger,
    IOrderClient orderClient
) : IOrderUseCase {
    public async Task CreateOrder(CreateOrderCommand order)
    {
        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("[INFO] - CreateOrderCommandHandler - Creating Order From {salesChannel}", order.Order.SalesChannel);
            logger.LogInformation("[INFO] - CreateOrderCommandHandler - Order: {order}", JsonSerializer.Serialize(order.Order));
        }
        await orderClient.CreateOrder(order.Order);
    }

    public async Task UpdateOrderStatus(UpdateOrderStatusCommand order)
    {
        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("[INFO] - UpdateOrderCommandHandler - Updating Order From: {salesChannel}", order.SalesChannel);
        }
        await orderClient.PatchOrder(order.OrderUpdate);
    }
}