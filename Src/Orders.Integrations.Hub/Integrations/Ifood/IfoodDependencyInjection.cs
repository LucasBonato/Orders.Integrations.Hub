using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.Out;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Ifood.Application.Clients;
using Orders.Integrations.Hub.Integrations.Ifood.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Ifood.Application.Ports.In;
using Orders.Integrations.Hub.Integrations.Ifood.Application.Ports.Out;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

namespace Orders.Integrations.Hub.Integrations.Ifood;

public static class IfoodDependencyInjection
{
    public static IServiceCollection AddIfood(this IServiceCollection services)
        => services
            .AddIfoodServices()
            .AddIfoodClients()
    ;

    private static IServiceCollection AddIfoodServices(this IServiceCollection services)
    {
        services.AddTransient<IOrderChangeStatusUseCase, IfoodOrderChangeStatusUseCase>();
        services.AddTransient<IOrderCreateUseCase<IfoodWebhookRequest>, IfoodOrderCreateUseCase>();
        services.AddTransient<IOrderUpdateUseCase<IfoodWebhookRequest>, IfoodOrderUpdateUseCase>();
        services.AddTransient<IOrderDisputeUseCase<IfoodWebhookRequest>, IfoodHandshakeOrderDisputeUseCase>();

        services.AddKeyedScoped<IOrderChangeStatusUseCase, IfoodOrderChangeStatusUseCase>(OrderIntegration.IFOOD);
        services.AddKeyedScoped<IOrderDisputeRespondUseCase, IfoodHandshakeOrderDisputeRespondUseCase>(OrderIntegration.IFOOD);
        services.AddKeyedScoped<IOrderChangeProductStatusUseCase, IfoodOrderChangeProductStatusUseCase>(OrderIntegration.IFOOD);
        services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, IfoodOrderGetCancellationReasonUseCase>(OrderIntegration.IFOOD);

        services.AddKeyedSingleton<ICustomJsonSerializer, CommonJsonSerializer>(OrderIntegration.IFOOD);

        return services;
    }

    private static IServiceCollection AddIfoodClients(this IServiceCollection services)
    {
        string baseUrl = AppEnv.INTEGRATIONS.IFOOD.ENDPOINT.BASE_URL.NotNullEnv();

        services.AddHttpClient<IIFoodAuthClient, IFoodAuthClient>(client => {
            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddScoped<IfoodAuthMessageHandler>();

        services.AddHttpClient<IIFoodClient, IFoodClient>(client => {
            client.BaseAddress = new Uri(baseUrl);
        }).AddHttpMessageHandler<IfoodAuthMessageHandler>();

        return services;
    }
}