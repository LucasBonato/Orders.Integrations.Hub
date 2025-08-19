using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.Clients;
using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs;

namespace Orders.Integrations.Hub.Core.Application.Clients;

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

    public async Task PatchOrder(OrderUpdate order)
    {
        logger.LogInformation("[INFO] - UpdateOrderEventHandler - Updating Order From Id: {orderId}", order.OrderId);

        HttpResponseMessage response = await httpClient.PatchAsJsonAsync("Orders", order);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - UpdateOrderEventHandler - Error updating order({orderId}): {statusCode}, {content}, {reason}", order.OrderId, response.StatusCode, response.Content.ReadAsStringAsync().Result, response.ReasonPhrase);
            throw new Exception();
        }
    }

    public async Task PatchOrderDispute(OrderUpdate order)
    {
        logger.LogInformation("[INFO] - UpdateOrderDisputeEventHandler - Updating Order Dispute From Id: {orderId}", order.OrderId);

        HttpResponseMessage response = await httpClient.PatchAsJsonAsync("Orders/dispute", order);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - UpdateOrderDisputeEventHandler - Error updating dispute of order({orderId}): {statusCode}, {content}, {reason}", order.OrderId, response.StatusCode, response.Content.ReadAsStringAsync().Result, response.ReasonPhrase);
            throw new Exception();
        }
    }
}