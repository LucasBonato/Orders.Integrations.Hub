using BizPik.Orders.Hub.Modules.Integrations.Ifood;

namespace BizPik.Orders.Hub.Modules.Integrations;

public static class IntegrationsModule
{
    public static IServiceCollection AddIntegrationsModule(this IServiceCollection services)
    {
        services.AddIfood();
        return services;
    }

    public static IApplicationBuilder UseIntegrationsModule(this WebApplication app)
    {
        app.AddIfoodEndpoints();
        return app;
    }
}