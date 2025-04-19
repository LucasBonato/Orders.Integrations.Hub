using BizPik.Orders.Hub.Modules.Integrations.Ifood;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Adapter;
using BizPik.Orders.Hub.Modules.Integrations.Rappi;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Adapter;

namespace BizPik.Orders.Hub.Modules.Integrations;

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