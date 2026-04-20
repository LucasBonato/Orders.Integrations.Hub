using Orders.Integrations.Hub.Integrations.Common.Middleware;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Infrastructure;

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
            .AddEndpointFilter<WebhookSignatureFilter<IFoodWebhookRequest, IFoodSignatureStrategy, IFoodSignatureStrategy>>()
        ;

        return app;
    }
}