using Microsoft.Extensions.Options;

using Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Infrastructure.Options;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class ClientsExtensions
{
    public static IServiceCollection AddClientsConfiguration(this IServiceCollection services, IConfiguration configuration) {
        services.AddOptions<ClientOptions>("Internal")
            .Bind(configuration.GetSection("Clients:Internal"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<ClientOptions>("Orders")
            .Bind(configuration.GetSection("Clients:Orders"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddHttpClient<IInternalClient, InternalClient>((serviceProvider, client) => {
            ClientOptions options = serviceProvider.GetRequiredService<IOptionsMonitor<ClientOptions>>().Get("Internal");
            client.BaseAddress = options.BaseUrl;
        });
        services.Decorate<IInternalClient, InternalCacheClient>();

        services.AddHttpClient<IOrderClient, OrderClient>((serviceProvider, client) => {
            ClientOptions options = serviceProvider.GetRequiredService<IOptionsMonitor<ClientOptions>>().Get("Orders");
            client.BaseAddress = options.BaseUrl;
        });

        return services;
    }
}