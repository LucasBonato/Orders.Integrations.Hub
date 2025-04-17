using BizPik.Orders.Hub.Modules.Integrations.Rappi.Adapter;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi;

public static class RappiEndpoints
{
    public static WebApplication AddRappiEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
                .MapGroup("/Rappi/Webhook")
                .WithTags("Ifood")
                .WithDescription("Ifood Webhook Endpoint")
            ;

        RouteGroupBuilder orders = routeGroup.MapGroup("/Orders");
        RouteGroupBuilder products = routeGroup.MapGroup("/Products");
        RouteGroupBuilder integrations = routeGroup.MapGroup("/Integrations");

        orders.MapPost("/", RappiWebhookAdapter.CreateOrder);
        orders.MapPost("/Auto-Accept", RappiWebhookAdapter.AutoAcceptOrder);
        orders.MapPost("/Cancel", RappiWebhookAdapter.CancelOrder);
        orders.MapPost("/Other", RappiWebhookAdapter.PatchOrder);
        orders.MapPost("/Ping", RappiWebhookAdapter.PingStore);

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