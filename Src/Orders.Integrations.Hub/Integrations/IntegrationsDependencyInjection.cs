using Orders.Integrations.Hub.Integrations.IFood;
using Orders.Integrations.Hub.Integrations.IFood.Adapter;
using Orders.Integrations.Hub.Integrations.Rappi;
using Orders.Integrations.Hub.Integrations.Rappi.Adapter;

namespace Orders.Integrations.Hub.Integrations;

public static class IntegrationsDependencyInjection
{
    public static IServiceCollection AddIntegrationsModule(this IServiceCollection services)
    {
        return services
                .AddServices()
                .AddIFood()
                .AddRappi()
            ;
    }

    public static WebApplication UseIntegrationsModule(this WebApplication app)
    {
        return app
                .UseIFoodEndpoints()
                .UseRappiEndpoints()
            ;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}