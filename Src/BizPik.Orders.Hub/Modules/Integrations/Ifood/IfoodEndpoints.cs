namespace BizPik.Orders.Hub.Modules.Integrations.Saleschannels.Adapters.Ifood;

public static class IfoodEndpoints
{
    public static WebApplication AddIfoodEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
            .MapGroup("/ifood")
        ;

        routeGroup.MapPost("/webhook", IfoodAdapter.Webhook);

        return app;
    }
}