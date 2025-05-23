namespace BizPik.Orders.Hub.Integrations.Ifood.Adapter;

public static class IfoodEndpoints
{
    public static WebApplication AddIfoodEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
                .MapGroup("/Ifood")
                .WithTags("Ifood")
                .WithDescription("Ifood Webhook Endpoint")
            ;

        routeGroup
            .MapPost("/Webhook", IfoodAdapter.Webhook)
            // .AddEndpointFilter<IfoodSignatureValidator>()
        ;

        return app;
    }
}