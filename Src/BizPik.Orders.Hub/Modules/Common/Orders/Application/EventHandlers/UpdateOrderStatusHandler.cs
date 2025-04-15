using BizPik.Orders.Hub.Modules.Common.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Common.Orders.Domain.ValueObjects.Events;

using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Application.EventHandlers;

public class UpdateOrderStatusHandler(
    ILogger<UpdateOrderStatusHandler> logger,
    HttpClient httpClient
) : IEventHandler<UpdateOrderStatusEvent>, IOrderHttp
{
    public async Task HandleAsync(UpdateOrderStatusEvent orderEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("[INFO] - UpdateOrderStatusHandler - Updating Order From: {salesChannel}; Id: {orderId}", orderEvent.SalesChannel, orderEvent.OrderUpdateStatus.OrderId);

        HttpResponseMessage response = await httpClient.PatchAsJsonAsync(
            requestUri: "api/Orders",
            value: orderEvent.OrderUpdateStatus,
            cancellationToken: cancellationToken
        );

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - UpdateOrderStatusHandler - Error updating order({orderId}): {statusCode}, {content}, {reason}", orderEvent.OrderUpdateStatus.OrderId, response.StatusCode, response.Content, response.ReasonPhrase);
            throw new Exception();
        }
    }
}