namespace Orders.Integrations.Hub.Integrations.IFood.Adapter;

public static class IFoodEndpoints
{
    public static WebApplication UseIFoodEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
                .MapGroup("/IFood")
                .WithTags("IFood")
                .WithDescription("IFood Webhook Endpoint")
            ;

        routeGroup
            .MapPost("/Webhook", IFoodAdapter.Webhook)
            // .AddEndpointFilter<IfoodSignatureValidator>()
        ;

        return app;
    }
}