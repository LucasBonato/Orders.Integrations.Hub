using BizPik.Orders.Hub.Modules.Common.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Common.Orders.Domain.ValueObjects.Events;
using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Application.EventHandlers;

public class CreateOrderHandler(
    ILogger<CreateOrderHandler> logger,
    HttpClient httpClient
) : IEventHandler<CreateOrderEvent>, IOrderHttp {
    public async Task HandleAsync(CreateOrderEvent orderEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("[INFO] - CreateOrderHandler - Creating Order From {salesChannel}", orderEvent.SalesChannel);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            requestUri: "api/Orders",
            value: orderEvent.Order,
            cancellationToken: cancellationToken
        );

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - CreateOrderHandler - Error creating order: {statusCode}, {content}, {reason}", response.StatusCode, response.Content, response.ReasonPhrase);
            throw new Exception();
        }
    }
}