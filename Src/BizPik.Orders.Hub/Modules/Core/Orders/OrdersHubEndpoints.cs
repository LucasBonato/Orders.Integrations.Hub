namespace BizPik.Orders.Hub.Modules.Core.Orders;

public static class OrdersHubEndpoints
{
    public static IApplicationBuilder AddOrdersHubEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
                .MapGroup("/Orders/Hub")
                .WithTags("Hub")
                .WithDescription("Change status for some integration")
            ;

        routeGroup
            .MapPatch("/", OrdersHubAdapter.ChangeIntegrationStatus)
        ;

        return app;
    }
}