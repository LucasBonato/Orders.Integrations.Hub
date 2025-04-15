using BizPik.Orders.Hub.Modules.Integrations.Ifood.Validators;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood;

public static class IfoodEndpoints
{
    public static WebApplication AddIfoodEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
            .MapGroup("/ifood")
            .WithTags("Ifood")
            .WithDescription("Ifood Webhook Endpoint")
        ;

        routeGroup.MapPost("/webhook", IfoodAdapter.Webhook).AddEndpointFilter<IfoodSignatureValidator>();

        return app;
    }
}