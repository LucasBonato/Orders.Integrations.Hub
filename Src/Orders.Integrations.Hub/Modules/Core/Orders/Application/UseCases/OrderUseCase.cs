using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Application.UseCases;

public class OrderUseCase(
    ILogger<OrderUseCase> logger,
    IOrderClient orderClient
) : IOrderUseCase {
    public async Task CreateOrder(CreateOrderEvent order)
    {
        logger.LogInformation("[INFO] - CreateOrderEventHandler - Creating Order From {salesChannel}", order.SalesChannel);
        await orderClient.CreateOrder(order.Order);
    }

    public async Task UpdateOrderStatus(UpdateOrderStatusEvent order)
    {
        logger.LogInformation("[INFO] - UpdateOrderStatusEventHandler - Updating Order From: {salesChannel}", order.SalesChannel);
        await orderClient.UpdateOrderStatus(order.OrderUpdateStatus);
    }
}