using System.Text.Json;

using Orders.Integrations.Hub.Core.Orders.Domain.Contracts;
using Orders.Integrations.Hub.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Core.Orders.Application.UseCases;

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
        logger.LogInformation("[INFO] - UpdateOrderStatusEventHandler - Updating Order From: {salesChannel}", order.SalesChannel);
        await orderClient.UpdateOrderStatus(order.OrderUpdateStatus);
    }
}