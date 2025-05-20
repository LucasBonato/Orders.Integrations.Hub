using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Core.Orders.Application.Extensions;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Clients;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Handlers;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Ports;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi;

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
        services.AddTransient<IOrderUpdateStatusUseCase<RappiWebhookEventOrderRequest>, RappiOrderUpdateStatusUseCase>();

        services.AddKeyedScoped<IOrderChangeStatusUseCase, RappiOrderChangeStatusUseCase>(OrderIntegration.RAPPI);
        services.AddKeyedScoped<IOrderChangeProductStatusUseCase, RappiOrderChangeProductStatusUseCase>(OrderIntegration.RAPPI);
        services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, RappiOrderGetCancellationReasonUseCase>(OrderIntegration.RAPPI);

        return services
                .Configure<JsonOptions>(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
            ;
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