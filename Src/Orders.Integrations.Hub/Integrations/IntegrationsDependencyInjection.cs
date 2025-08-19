using Orders.Integrations.Hub.Integrations.Ifood;
using Orders.Integrations.Hub.Integrations.Ifood.Adapter;
using Orders.Integrations.Hub.Integrations.Rappi;
using Orders.Integrations.Hub.Integrations.Rappi.Adapter;

namespace Orders.Integrations.Hub.Integrations;

public static class IntegrationsDependencyInjection
{
    public static IServiceCollection AddIntegrationsModule(this IServiceCollection services)
    {
        return services
                .AddServices()
                .AddIfood()
                .AddRappi()
            ;
    }

    public static IApplicationBuilder UseIntegrationsModule(this WebApplication app)
    {
        return app
                .UseIfoodEndpoints()
                .UseRappiEndpoints()
            ;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}