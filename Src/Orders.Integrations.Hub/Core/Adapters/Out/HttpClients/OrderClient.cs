using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Domain.Entity;

namespace Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;

public class OrderClient(
    ILogger<OrderClient> logger,
    HttpClient httpClient
) : IOrderClient
{
    public async Task CreateOrder(Order order)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("[INFO] - CreateOrderCommandHandler - Creating Order, Id: {orderId}", order.OrderId);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("Orders", order);

        if (!response.IsSuccessStatusCode)
        {
            if (logger.IsEnabled(LogLevel.Error))
                logger.LogError("[ERROR] - CreateOrderCommandHandler - Error creating order: {statusCode}, {content}, {reason}", response.StatusCode, response.Content.ReadAsStringAsync().Result, response.ReasonPhrase);
            throw new Exception();
        }
    }

    public async Task PatchOrder(OrderUpdate order)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("[INFO] - UpdateOrderCommandHandler - Updating Order From Id: {orderId}", order.OrderId);

        HttpResponseMessage response = await httpClient.PatchAsJsonAsync("Orders", order);

        if (!response.IsSuccessStatusCode)
        {
            if (logger.IsEnabled(LogLevel.Error))
                logger.LogError("[ERROR] - UpdateOrderCommandHandler - Error updating order({orderId}): {statusCode}, {content}, {reason}", order.OrderId, response.StatusCode, response.Content.ReadAsStringAsync().Result, response.ReasonPhrase);
            throw new Exception();
        }
    }

    public async Task PatchOrderDispute(OrderUpdate order)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("[INFO] - UpdateOrderDisputeEventHandler - Updating Order Dispute From Id: {orderId}", order.OrderId);

        HttpResponseMessage response = await httpClient.PatchAsJsonAsync("Orders/dispute", order);

        if (!response.IsSuccessStatusCode)
        {
            if (logger.IsEnabled(LogLevel.Error))
                logger.LogError("[ERROR] - UpdateOrderDisputeEventHandler - Error updating dispute of order({orderId}): {statusCode}, {content}, {reason}", order.OrderId, response.StatusCode, response.Content.ReadAsStringAsync().Result, response.ReasonPhrase);
            throw new Exception();
        }
    }
}