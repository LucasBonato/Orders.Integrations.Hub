namespace Orders.Integrations.Hub.Integrations.Ifood.Adapter;

public static class IfoodEndpoints
{
    public static WebApplication UseIfoodEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
                .MapGroup("/Ifood")
                .WithTags("Ifood")
                .WithDescription("Ifood Webhook Endpoint")
            ;

        routeGroup
            .MapPost("/Webhook", IFoodAdapter.Webhook)
            // .AddEndpointFilter<IfoodSignatureValidator>()
        ;

        return app;
    }
}