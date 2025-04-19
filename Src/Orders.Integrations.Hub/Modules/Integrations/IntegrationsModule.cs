using Orders.Integrations.Hub.Modules.Integrations.Ifood;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Adapter;
using Orders.Integrations.Hub.Modules.Integrations.Rappi;
using Orders.Integrations.Hub.Modules.Integrations.Rappi.Adapter;

namespace Orders.Integrations.Hub.Modules.Integrations;

public static class IntegrationsModule
{
    public static IServiceCollection AddIntegrationsModule(this IServiceCollection services)
    {
        return services
            .AddIfood()
            .AddRappi()
        ;
    }

    public static IApplicationBuilder UseIntegrationsModule(this WebApplication app)
    {
        return app
            .AddIfoodEndpoints()
            .AddRappiEndpoints()
        ;
    }
}