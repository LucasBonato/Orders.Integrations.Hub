using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.Clients;

public class OrderClient(
    ILogger<OrderClient> logger,
    HttpClient httpClient
) : IOrderClient
{
    public async Task CreateOrder(Order order)
    {
        logger.LogInformation("[INFO] - CreateOrderEventHandler - Creating Order, Id: {orderId}", order.OrderId);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("Orders", order);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - CreateOrderEventHandler - Error creating order: {statusCode}, {content}, {reason}", response.StatusCode, response.Content.ReadAsStringAsync().Result, response.ReasonPhrase);
            throw new Exception();
        }
    }

    public async Task UpdateOrderStatus(OrderUpdateStatus order)
    {
        logger.LogInformation("[INFO] - UpdateOrderStatusEventHandler - Updating Order From Id: {orderId}", order.OrderId);

        HttpResponseMessage response = await httpClient.PatchAsJsonAsync("Orders", order);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - UpdateOrderStatusEventHandler - Error updating order({orderId}): {statusCode}, {content}, {reason}", order.OrderId, response.StatusCode, response.Content.ReadAsStringAsync().Result, response.ReasonPhrase);
            throw new Exception();
        }
    }
}