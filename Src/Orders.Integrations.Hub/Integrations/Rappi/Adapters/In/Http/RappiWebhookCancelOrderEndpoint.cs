using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Adapters.In.Http.Endpoint;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Integrations.Common.Middleware;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Infrastructure;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.Rappi.Adapters.In.Http;

internal sealed class RappiWebhookCancelOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/Rappi/Webhook/Cancel", async (
            [FromServices] IOrderUpdateUseCase<RappiWebhookEventOrderRequest> orderUpdate,
            HttpContext context
        ) => {
            RappiWebhookEventOrderRequest request = (RappiWebhookEventOrderRequest)context.Items["WebhookRequest"]!;
            await orderUpdate.ExecuteAsync(request);
            return Accepted();
        })
        .WithTags("Rappi")
        .WithDescription("Rappi Webhook Endpoint")
        .ProducesValidationProblem()
        .AddEndpointFilter<WebhookSignatureFilter<RappiWebhookEventOrderRequest, RappiSignatureValidator, RappiOrderEventResolver>>();
    }
}