using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Clients;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Handlers;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Ports;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

using Microsoft.AspNetCore.Mvc;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood;

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
        services.AddTransient<IOrderUpdateStatusUseCase<IfoodWebhookRequest>, IfoodOrderUpdateStatusUseCase>();

        services.AddKeyedScoped<IOrderChangeStatusUseCase, IfoodOrderChangeStatusUseCase>(OrderIntegration.IFOOD);
        services.AddKeyedScoped<IOrderChangeProductStatusUseCase, IfoodOrderChangeProductStatusUseCase>(OrderIntegration.IFOOD);
        services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, IfoodOrderGetCancellationReasonUseCase>(OrderIntegration.IFOOD);

        return services
                .Configure<JsonOptions>(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
            ;
    }

    private static IServiceCollection AddIfoodClients(this IServiceCollection services)
    {
        string baseUrl = AppEnv.INTEGRATIONS.IFOOD.ENDPOINT.BASE_URL.NotNull();

        services.AddHttpClient<IfoodAuthClient, IfoodAuthClient>(client => {
            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddScoped<IfoodAuthMessageHandler>();

        services.AddHttpClient<IIFoodClient, IfoodClient>(client => {
            client.BaseAddress = new Uri(baseUrl);
        }).AddHttpMessageHandler<IfoodAuthMessageHandler>();

        return services;
    }
}