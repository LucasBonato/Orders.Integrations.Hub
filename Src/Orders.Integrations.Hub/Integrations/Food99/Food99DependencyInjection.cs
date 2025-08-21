﻿using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.In;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Integrations.Out;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
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

        services.AddKeyedScoped<IOrderChangeStatusUseCase, Food99OrderChangeStatusUseCase>(OrderIntegration.FOOD99); 
        services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, Food99OrderGetCancellationReasonUseCase>(OrderIntegration.FOOD99);        

        services.AddKeyedSingleton<ICustomJsonSerializer, Food99JsonSerializer>(OrderIntegration.FOOD99);

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