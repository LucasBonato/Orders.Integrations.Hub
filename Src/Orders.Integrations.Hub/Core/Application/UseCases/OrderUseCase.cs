using System.Text.Json;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.Core.Application.UseCases;

public class OrderUseCase(
    ILogger<OrderUseCase> logger,
    IOrderClient orderClient
) : IOrderUseCase {
    public async Task CreateOrder(CreateOrderEvent order)
    {
        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("[INFO] - CreateOrderEventHandler - Creating Order From {salesChannel}", order.SalesChannel);
            logger.LogInformation("[INFO] - CreateOrderEventHandler - Order: {order}", JsonSerializer.Serialize(order.Order));
        }
        await orderClient.CreateOrder(order.Order);
    }

    public async Task UpdateOrderStatus(UpdateOrderStatusEvent order)
    {
        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("[INFO] - UpdateOrderEventHandler - Updating Order From: {salesChannel}", order.SalesChannel);
        }
        await orderClient.PatchOrder(order.OrderUpdate);
    }
}