namespace BizPik.Orders.Hub.Integrations.Rappi.Adapter;

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

        webhook.MapPost("/", RappiWebhookAdapter.CreateOrder);
        webhook.MapPost("/Auto-Accept", RappiWebhookAdapter.AutoAcceptOrder);
        webhook.MapPost("/Cancel", RappiWebhookAdapter.CancelOrder);
        webhook.MapPost("/Other", RappiWebhookAdapter.PatchOrder);
        webhook.MapPost("/Ping", RappiWebhookAdapter.PingStore);

        return app;
    }
}