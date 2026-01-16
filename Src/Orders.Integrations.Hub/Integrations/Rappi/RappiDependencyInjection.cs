using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.Out;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Clients;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.In;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.Out;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Rappi;

public static class RappiDependencyInjection
{
    public static IServiceCollection AddRappi(this IServiceCollection services)
        => services
            .AddRappiServices()
            .AddRappiClients()
        ;

    private static IServiceCollection AddRappiServices(this IServiceCollection services)
    {
        services.AddTransient<IOrderCreateUseCase<RappiOrder>, RappiOrderCreateUseCase>();
        services.AddTransient<IOrderUpdateUseCase<RappiWebhookEventOrderRequest>, RappiOrderUpdateUseCase>();

        services.AddKeyedScoped<IOrderChangeStatusUseCase, RappiOrderChangeStatusUseCase>(RappiIntegrationKey.Value);
        services.AddKeyedScoped<IOrderChangeProductStatusUseCase, RappiOrderChangeProductStatusUseCase>(RappiIntegrationKey.Value);
        services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, RappiOrderGetCancellationReasonUseCase>(RappiIntegrationKey.Value);

        services.AddKeyedSingleton<ICustomJsonSerializer, RappiJsonSerializer>(RappiIntegrationKey.Value);

        return services;
    }

    private static IServiceCollection AddRappiClients(this IServiceCollection services)
    {
        string baseUrl = AppEnv.INTEGRATIONS.RAPPI.ENDPOINT.BASE_URL.NotNullEnv();
        string baseAuthUrl = AppEnv.INTEGRATIONS.RAPPI.ENDPOINT.AUTH.NotNullEnv();

        services.AddHttpClient<RappiAuthClient, RappiAuthClient>(client => {
            client.BaseAddress = new Uri(baseAuthUrl);
        });

        services.AddScoped<RappiAuthMessageHandler>();

        services.AddHttpClient<IRappiClient, RappiClient>(client => {
            client.BaseAddress = new Uri(baseUrl);
        }).AddHttpMessageHandler<RappiAuthMessageHandler>();

        return services;
    }
}