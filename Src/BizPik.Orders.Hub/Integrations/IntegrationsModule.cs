using BizPik.Orders.Hub.Integrations.Common.Application.Services;
using BizPik.Orders.Hub.Integrations.Common.Contracts;
using BizPik.Orders.Hub.Integrations.Ifood;
using BizPik.Orders.Hub.Integrations.Ifood.Adapter;
using BizPik.Orders.Hub.Integrations.Rappi;
using BizPik.Orders.Hub.Integrations.Rappi.Adapter;

namespace BizPik.Orders.Hub.Integrations;

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
                .UseIfoodEndpoints()
                .UseRappiEndpoints()
            ;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
                .AddSingleton<ICacheService, MemoryCacheService>()
            ;
    }

    private static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        return services
                .AddMemoryCache()
            ;
    }
}