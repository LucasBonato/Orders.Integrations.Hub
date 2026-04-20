using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.Rappi.Adapter;

public class RappiWebhookAdapter {
    public static async Task<IResult> CreateOrder(
        [FromServices] IOrderCreateUseCase<RappiOrder> orderCreate,
        HttpContext context
    ) {
        RappiOrder request = (RappiOrder)context.Items["WebhookRequest"]!;
        await orderCreate.ExecuteAsync(request);
        return Created();
    }

    public static async Task<IResult> CancelOrder(
        [FromServices] IOrderUpdateUseCase<RappiWebhookEventOrderRequest> orderUpdate,
        HttpContext context
    ) {
        RappiWebhookEventOrderRequest request = (RappiWebhookEventOrderRequest)context.Items["WebhookRequest"]!;
        await orderUpdate.ExecuteAsync(request);
        return Accepted();
    }

    public static async Task<IResult> PatchOrder(
        [FromServices] IOrderUpdateUseCase<RappiWebhookEventOrderRequest> orderUpdate,
        HttpContext context
    ) {
        RappiWebhookEventOrderRequest request = (RappiWebhookEventOrderRequest)context.Items["WebhookRequest"]!;
        await orderUpdate.ExecuteAsync(request);
        return Accepted();
    }

    public static IResult PingStore(
        HttpContext context
    ) {
        RappiWebhookPingRequest _ = (RappiWebhookPingRequest)context.Items["WebhookRequest"]!;

        // TODO - Manage better this response finding somewhere if the store is on

        return Ok(new RappiWebhookPingResponse(
            Status: "Ok",
            Description: "Store on"
        ));
    }
}