using Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class ClientsExtensions
{
    public static IServiceCollection AddClientsConfiguration(this IServiceCollection services) {
        services.AddHttpClient<IInternalClient, InternalClient>(client => {
            client.BaseAddress = new Uri(AppEnv.INTERNAL.ENDPOINT.BASE_URL.NotNullEnv());
        });
        services.Decorate<IInternalClient, InternalCacheClient>();

        services.AddHttpClient<IOrderClient, OrderClient>(client => {
            client.BaseAddress = new Uri(AppEnv.ORDERS.ENDPOINT.BASE_URL.NotNullEnv());
        });

        return services;
    }
}