using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;

using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.EventHandlers;

public class UpdateOrderStatusEventHandler(
    ILogger<UpdateOrderStatusEventHandler> logger,
    HttpClient httpClient
) : IEventHandler<UpdateOrderStatusEvent>, IOrderHttp
{
    public async Task HandleAsync(UpdateOrderStatusEvent orderEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("[INFO] - UpdateOrderStatusEventHandler - Updating Order From: {salesChannel}; Id: {orderId}", orderEvent.SalesChannel, orderEvent.OrderUpdateStatus.OrderId);

        HttpResponseMessage response = await httpClient.PatchAsJsonAsync(
            requestUri: "api/Orders",
            value: orderEvent.OrderUpdateStatus,
            cancellationToken: cancellationToken
        );

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - UpdateOrderStatusEventHandler - Error updating order({orderId}): {statusCode}, {content}, {reason}", orderEvent.OrderUpdateStatus.OrderId, response.StatusCode, response.Content, response.ReasonPhrase);
            throw new Exception();
        }
    }
}