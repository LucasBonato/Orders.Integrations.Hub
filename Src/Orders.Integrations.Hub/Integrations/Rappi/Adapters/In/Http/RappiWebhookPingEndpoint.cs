using Orders.Integrations.Hub.Core.Application.Ports.In.Http;
using Orders.Integrations.Hub.Integrations.Common.Middleware;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;
using Orders.Integrations.Hub.Integrations.Rappi.Infrastructure;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.Rappi.Adapters.In.Http;

internal sealed class RappiWebhookPingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/Rappi/Webhook/Ping", (
            HttpContext context
        ) => {
            RappiWebhookPingRequest _ = (RappiWebhookPingRequest)context.Items["WebhookRequest"]!;

            // TODO - Manage better this response finding somewhere if the store is on

            return Ok(new RappiWebhookPingResponse(
                Status: "Ok",
                Description: "Store on"
            ));
        })
        .WithTags("Rappi")
        .WithDescription("Rappi Webhook Endpoint")
        .ProducesValidationProblem()
        .AddEndpointFilter<WebhookSignatureFilter<RappiWebhookPingRequest, RappiSignatureValidator, RappiPingResolver>>();
    }
}