namespace Orders.Integrations.Hub.Core.Infrastructure;

public static class OrdersHubRouteConfiguration
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

        routeGroup.MapGet("/Cancellation-Reason", OrdersHubController.GetIntegrationCancellationReason);

        routeGroupStatus.MapPatch("/", OrdersHubController.ChangeIntegrationStatus);

        routeGroupProductStatus.MapPost("/Enable", OrdersHubController.EnableIntegrationProduct);
        routeGroupProductStatus.MapPost("/Disable", OrdersHubController.DisableIntegrationProduct);

        routeGroupDispute.MapPost("/", OrdersHubController.PostResponseDisputeIntegration);

        return app;
    }
}