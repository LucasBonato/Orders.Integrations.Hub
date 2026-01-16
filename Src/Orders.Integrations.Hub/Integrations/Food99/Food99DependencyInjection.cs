using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.In;
using Orders.Integrations.Hub.Core.Domain.Contracts.Ports.Out;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;
using Orders.Integrations.Hub.Integrations.Food99.Application.Clients;
using Orders.Integrations.Hub.Integrations.Food99.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Food99.Application.Ports.In;
using Orders.Integrations.Hub.Integrations.Food99.Application.Ports.Out;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;

namespace Orders.Integrations.Hub.Integrations.Food99;

public static class Food99DependencyInjection
{
    public static IServiceCollection AddFood99(this IServiceCollection services)
        => services
            .AddFood99Services()
            .AddFood99Clients()
        ;

    private static IServiceCollection AddFood99Services(this IServiceCollection services)
    {
        services.AddTransient<IOrderCreateUseCase<Food99WebhookRequest>, Food99OrderCreateUseCase>();
        services.AddTransient<IOrderUpdateUseCase<Food99WebhookRequest>, Food99OrderUpdateUseCase>();
        services.AddTransient<IOrderDisputeUseCase<Food99WebhookRequest>, Food99ApplyOrderDisputeUseCase>();

        services.AddKeyedScoped<IOrderChangeStatusUseCase, Food99OrderChangeStatusUseCase>(Food99IntegrationKey.Value);
        services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, Food99OrderGetCancellationReasonUseCase>(Food99IntegrationKey.Value);

        services.AddKeyedSingleton<ICustomJsonSerializer, Food99JsonSerializer>(Food99IntegrationKey.Value);

        return services;
    }

    private static IServiceCollection AddFood99Clients(this IServiceCollection services)
    {
        string baseUrl = AppEnv.INTEGRATIONS.FOOD99.ENDPOINT.BASE_URL.NotNullEnv();

        services.AddHttpClient<IFood99AuthClient, Food99AuthClient>(client => {
            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddScoped<Food99AuthMessageHandler>();

        services.AddHttpClient<IFood99Client, Food99Client>(client => {
            client.BaseAddress = new Uri(baseUrl);
        }).AddHttpMessageHandler<Food99AuthMessageHandler>();

        return services;
    }
}