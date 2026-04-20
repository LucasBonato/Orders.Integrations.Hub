using Orders.Integrations.Hub.Integrations.Common.Middleware;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Infrastructure;

namespace Orders.Integrations.Hub.Integrations.Rappi.Adapter;

public static class RappiEndpoints
{
    public static WebApplication UseRappiEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
                .MapGroup("/Rappi")
                .WithTags("Rappi")
                .WithDescription("Rappi Webhook Endpoint")
            ;

        RouteGroupBuilder webhook = routeGroup
                .MapGroup("/Webhook")
                .WithTags("Rappi")
                .WithDescription("Rappi Webhook Endpoint")
            ;

        webhook
            .MapPost("/", RappiWebhookAdapter.CreateOrder)
            .AddEndpointFilter<WebhookSignatureFilter<RappiOrder, RappiSignatureValidator, RappiOrderResolver>>();
        webhook
            .MapPost("/Cancel", RappiWebhookAdapter.CancelOrder)
            .AddEndpointFilter<WebhookSignatureFilter<RappiWebhookEventOrderRequest, RappiSignatureValidator, RappiOrderEventResolver>>();
        webhook
            .MapPost("/Other", RappiWebhookAdapter.PatchOrder)
            .AddEndpointFilter<WebhookSignatureFilter<RappiWebhookEventOrderRequest, RappiSignatureValidator, RappiOrderEventResolver>>();
        webhook
            .MapPost("/Ping", RappiWebhookAdapter.PingStore)
            .AddEndpointFilter<WebhookSignatureFilter<RappiWebhookPingRequest, RappiSignatureValidator, RappiPingResolver>>();

        return app;
    }
}