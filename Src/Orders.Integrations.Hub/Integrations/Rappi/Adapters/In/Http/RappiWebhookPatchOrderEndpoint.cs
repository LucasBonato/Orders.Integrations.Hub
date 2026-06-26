using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Ports.In.Http;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Integrations.Common.Middleware;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Infrastructure;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.Rappi.Adapters.In.Http;

internal sealed class RappiWebhookPatchOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/rappi/webhook/other", async (
            [FromServices] IOrderUpdateUseCase<RappiWebhookEventOrderRequest> orderUpdate,
            HttpContext context
        ) => {
            RappiWebhookEventOrderRequest request = (RappiWebhookEventOrderRequest)context.Items["WebhookRequest"]!;
            await orderUpdate.ExecuteAsync(request);
            return Accepted();
        })
        .WithTags("Rappi")
        .WithDescription("Rappi webhook endpoint to receive other events")
        .Accepts<RappiWebhookEventOrderRequest>("application/json")
        .Produces(StatusCodes.Status202Accepted)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .AddEndpointFilter<WebhookSignatureFilter<RappiWebhookEventOrderRequest, RappiSignatureValidator, RappiOrderEventResolver>>();
    }
}