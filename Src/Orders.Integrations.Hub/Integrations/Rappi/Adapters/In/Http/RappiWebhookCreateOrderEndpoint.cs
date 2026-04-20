using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Adapters.In.Http.Endpoint;
using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Integrations.Common.Middleware;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Rappi.Infrastructure;

using static Microsoft.AspNetCore.Http.Results;

namespace Orders.Integrations.Hub.Integrations.Rappi.Adapters.In.Http;

internal sealed class RappiWebhookCreateOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/Rappi/Webhook", async (
            [FromServices] IOrderCreateUseCase<RappiOrder> orderCreate,
            HttpContext context
        ) => {
            RappiOrder request = (RappiOrder)context.Items["WebhookRequest"]!;
            await orderCreate.ExecuteAsync(request);
            return Created();
        })
        .WithTags("Rappi")
        .WithDescription("Rappi Webhook Endpoint")
        .ProducesValidationProblem()
        .AddEndpointFilter<WebhookSignatureFilter<RappiOrder, RappiSignatureValidator, RappiOrderResolver>>();
    }
}