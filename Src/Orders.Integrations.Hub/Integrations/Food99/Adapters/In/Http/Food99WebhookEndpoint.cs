using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Ports.In.Http;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Integrations.Common.Middleware;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Food99.Infrastructure;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.Food99.Adapters.In.Http;

internal sealed class Food99WebhookEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/Food99/Webhook", async (
            [FromServices] IOrderCreateUseCase<Food99WebhookRequest> orderCreate,
            [FromServices] IOrderUpdateUseCase<Food99WebhookRequest> orderUpdate,
            [FromServices] IOrderDisputeUseCase<Food99WebhookRequest> orderDispute,
            HttpContext context
        ) => {
            Food99WebhookRequest request = (Food99WebhookRequest)context.Items["WebhookRequest"]!;

            Food99WebhookRequest? result = request.Type switch {
                Food99Type.OrderNew => await orderCreate.ExecuteAsync(request),

                Food99Type.DeliveryStatus or
                Food99Type.OrderCancel or
                Food99Type.OrderPartialCancel or
                Food99Type.OrderFinish => await orderUpdate.ExecuteAsync(request),

                Food99Type.OrderCancelApply or
                Food99Type.OrderRefundApply => await orderDispute.ExecuteAsync(request),
                
                _ => null
            };

            return result is null
                ? BadRequest(new { Errno = 1, Errmsg = "Could not detect webhook request" })
                : Ok(new { Errno = 0, Errmsg = "ok" });
        })
        .WithTags("Food99")
        .WithDescription("Food99 Webhook Endpoint")
        .Produces<Food99BaseResponse>()
        .ProducesValidationProblem()
        .AddEndpointFilter<WebhookSignatureFilter<Food99WebhookRequest, Food99SignatureStrategy, Food99SignatureStrategy>>();
    }
}