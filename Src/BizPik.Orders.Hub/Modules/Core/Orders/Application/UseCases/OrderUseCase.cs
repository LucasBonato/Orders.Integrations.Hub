using System.Text.Json;

using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.UseCases;

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