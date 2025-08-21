using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.Out;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.IFood.Application.Clients;
using Orders.Integrations.Hub.Integrations.IFood.Application.Handlers;
using Orders.Integrations.Hub.Integrations.IFood.Application.Ports.In;
using Orders.Integrations.Hub.Integrations.IFood.Application.Ports.Out;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.IFood;

public static class IFoodDependencyInjection
{
    public static IServiceCollection AddIFood(this IServiceCollection services)
        => services
            .AddIFoodServices()
            .AddIFoodClients()
    ;

    private static IServiceCollection AddIFoodServices(this IServiceCollection services)
    {
        services.AddTransient<IOrderChangeStatusUseCase, IFoodOrderChangeStatusUseCase>();
        services.AddTransient<IOrderCreateUseCase<IFoodWebhookRequest>, IFoodOrderCreateUseCase>();
        services.AddTransient<IOrderUpdateUseCase<IFoodWebhookRequest>, IFoodOrderUpdateUseCase>();
        services.AddTransient<IOrderDisputeUseCase<IFoodWebhookRequest>, IFoodHandshakeOrderDisputeUseCase>();

        services.AddKeyedScoped<IOrderChangeStatusUseCase, IFoodOrderChangeStatusUseCase>(OrderIntegration.IFOOD);
        services.AddKeyedScoped<IOrderDisputeRespondUseCase, IFoodHandshakeOrderDisputeRespondUseCase>(OrderIntegration.IFOOD);
        services.AddKeyedScoped<IOrderChangeProductStatusUseCase, IFoodOrderChangeProductStatusUseCase>(OrderIntegration.IFOOD);
        services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, IFoodOrderGetCancellationReasonUseCase>(OrderIntegration.IFOOD);

        services.AddKeyedSingleton<ICustomJsonSerializer, CommonJsonSerializer>(OrderIntegration.IFOOD);

        return services;
    }

    private static IServiceCollection AddIFoodClients(this IServiceCollection services)
    {
        string baseUrl = AppEnv.INTEGRATIONS.IFOOD.ENDPOINT.BASE_URL.NotNullEnv();

        services.AddHttpClient<IIFoodAuthClient, IFoodAuthClient>(client => {
            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddScoped<IFoodAuthMessageHandler>();

        services.AddHttpClient<IIFoodClient, IFoodClient>(client => {
            client.BaseAddress = new Uri(baseUrl);
        }).AddHttpMessageHandler<IFoodAuthMessageHandler>();

        return services;
    }
}