namespace BizPik.Orders.Hub.Core.Adapter;

public static class OrdersHubEndpoints
{
    public static IApplicationBuilder AddOrdersHubEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
                .MapGroup("/Orders/Hub")
                .WithTags("Hub")
                .WithDescription("Change status for integration")
            ;

        RouteGroupBuilder routeGroupStatus = routeGroup
                .MapGroup("/Status")
                .WithTags("Change Order Status")
                .WithDescription("Change order status in integrations")
            ;

        RouteGroupBuilder routeGroupProductStatus = routeGroup
                .MapGroup("/Product")
                .WithTags("Product Status")
                .WithDescription("Change status of the products in integrations")
            ;

        RouteGroupBuilder routeGroupDispute = routeGroup
                .MapGroup("/Dispute")
                .WithTags("Change Order Dispute Status")
                .WithDescription("Change order dispute in integrations")
            ;

        routeGroup.MapGet("/Cancellation-Reason", OrdersHubAdapter.GetIntegrationCancellationReason);

        routeGroupStatus.MapPatch("/", OrdersHubAdapter.ChangeIntegrationStatus);

        routeGroupProductStatus.MapPost("/Enable", OrdersHubAdapter.EnableIntegrationProduct);
        routeGroupProductStatus.MapPost("/Disable", OrdersHubAdapter.DisableIntegrationProduct);

        routeGroupDispute.MapPost("/", OrdersHubAdapter.PostResponseDisputeIntegration);

        return app;
    }
}