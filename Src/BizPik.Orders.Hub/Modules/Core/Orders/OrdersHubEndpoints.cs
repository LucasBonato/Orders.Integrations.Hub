namespace BizPik.Orders.Hub.Modules.Core.Orders;

public static class OrdersHubEndpoints
{
    public static IApplicationBuilder AddOrdersHubEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
                .MapGroup("/Orders/Hub")
                .WithTags("Hub")
                .WithDescription("Change status for integration")
            ;

        RouteGroupBuilder routeGroupProductStatus = routeGroup
                .MapGroup("/Product")
                .WithTags("Product Status")
                .WithDescription("Change status of the products for integration")
            ;

        routeGroup.MapPatch("/", OrdersHubAdapter.ChangeIntegrationStatus);

        routeGroupProductStatus.MapPost("/Enable", OrdersHubAdapter.EnableIntegrationProduct);
        routeGroupProductStatus.MapPost("/Disable", OrdersHubAdapter.DisableIntegrationProduct);

        return app;
    }
}