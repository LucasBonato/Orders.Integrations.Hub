namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Adapter;

public static class RappiEndpoints
{
    public static WebApplication AddRappiEndpoints(this WebApplication app)
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

        // RouteGroupBuilder orders = routeGroup.MapGroup("/Orders");
        // RouteGroupBuilder products = routeGroup.MapGroup("/Products");
        // RouteGroupBuilder integrations = routeGroup.MapGroup("/Integrations");
        // webhook.MapPost("/Status", RappiWebhookAdapter.WebhookCreate);
        // webhook.MapPost("/Orders", RappiWebhookAdapter.WebhookCreate);
        // webhook.MapPost("/Orders", RappiWebhookAdapter.WebhookCreate);
        // webhook.MapPost("/Orders", RappiWebhookAdapter.WebhookCreate);
        // webhook.MapPost("/Orders", RappiWebhookAdapter.WebhookCreate);
        // webhook.MapPost("/Orders", RappiWebhookAdapter.WebhookCreate);
        // webhook.MapPost("/Orders", RappiWebhookAdapter.WebhookCreate);
        // webhook.MapPost("/Orders", RappiWebhookAdapter.WebhookCreate);

        return app;
    }
}