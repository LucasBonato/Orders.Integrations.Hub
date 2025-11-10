using System.Text.Json;

using Orders.Integrations.Hub.Core.Application.Events;
using Orders.Integrations.Hub.Core.Domain.Contracts.Clients;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Core;

namespace Orders.Integrations.Hub.Core.Application.UseCases;

public class OrderUseCase(
    ILogger<OrderUseCase> logger,
    IOrderClient orderClient
) : IOrderUseCase, IOrderHttp {
    public async Task CreateOrder(CreateOrderEvent order)
    {
        logger.LogInformation("[INFO] - CreateOrderEventHandler - Creating Order From {salesChannel}", order.SalesChannel);
        logger.LogInformation("[INFO] - CreateOrderEventHandler - Order: {order}", JsonSerializer.Serialize(order.Order));
        await orderClient.CreateOrder(order.Order);
    }

    public async Task UpdateOrderStatus(UpdateOrderStatusEvent order)
    {
        logger.LogInformation("[INFO] - UpdateOrderEventHandler - Updating Order From: {salesChannel}", order.SalesChannel);
        await orderClient.PatchOrder(order.OrderUpdate);
    }
}