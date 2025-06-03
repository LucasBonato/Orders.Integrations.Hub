using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Orders.Application.Extensions;
using BizPik.Orders.Hub.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Integrations.Ifood.Application.Clients;
using BizPik.Orders.Hub.Integrations.Ifood.Application.Handlers;
using BizPik.Orders.Hub.Integrations.Ifood.Application.Ports;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

using Microsoft.AspNetCore.Mvc;

namespace BizPik.Orders.Hub.Integrations.Ifood;

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

        return services
                .Configure<JsonOptions>(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
            ;
    }

    private static IServiceCollection AddIfoodClients(this IServiceCollection services)
    {
        string baseUrl = AppEnv.INTEGRATIONS.IFOOD.ENDPOINT.BASE_URL.NotNullEnv();

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