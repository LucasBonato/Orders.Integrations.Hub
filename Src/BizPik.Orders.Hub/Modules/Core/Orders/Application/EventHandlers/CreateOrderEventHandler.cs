using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Events;
using FastEndpoints;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.EventHandlers;

public class CreateOrderEventHandler(
    ILogger<CreateOrderEventHandler> logger,
    HttpClient httpClient
) : IEventHandler<CreateOrderEvent>, IOrderHttp {
    public async Task HandleAsync(CreateOrderEvent orderEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("[INFO] - CreateOrderEventHandler - Creating Order From {salesChannel}", orderEvent.SalesChannel);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            requestUri: "api/Orders",
            value: orderEvent.Order,
            cancellationToken: cancellationToken
        );

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("[ERROR] - CreateOrderEventHandler - Error creating order: {statusCode}, {content}, {reason}", response.StatusCode, response.Content, response.ReasonPhrase);
            throw new Exception();
        }
    }
}