namespace Orders.Integrations.Hub.Integrations.Food99.Adapter;

public static class Food99Endpoints
{
    public static WebApplication UseFood99Endpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
                .MapGroup("/Food99")
                .WithTags("Food99")
                .WithDescription("Food99 Webhook Endpoint")
            ;

        RouteGroupBuilder webhook = routeGroup
                .MapGroup("/Webhook")
                .WithTags("Food99")
                .WithDescription("Food99 Webhook Endpoint")
            ;

        webhook.MapPost("/", Food99WebhookAdapter.Webhook);

        return app;
    }
}