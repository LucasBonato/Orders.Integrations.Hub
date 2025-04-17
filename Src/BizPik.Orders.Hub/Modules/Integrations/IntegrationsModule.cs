using BizPik.Orders.Hub.Modules.Integrations.Ifood;

namespace BizPik.Orders.Hub.Modules.Integrations;

public static class IntegrationsModule
{
    public static IServiceCollection AddIntegrationsModule(this IServiceCollection services)
    {
        return services
            .AddIfood()
        ;
    }

    public static IApplicationBuilder UseIntegrationsModule(this WebApplication app)
    {
        return app
            .AddIfoodEndpoints()
        ;
    }
}