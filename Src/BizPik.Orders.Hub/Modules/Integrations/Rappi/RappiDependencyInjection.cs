using System.Text.Json.Serialization;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Clients;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Handlers;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Ports;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

using Microsoft.AspNetCore.Mvc;

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
        services.AddTransient<ICreateOrderUseCase<RappiOrder>, RappiCreateOrderUseCase>();
        services.AddTransient<IUpdateOrderStatusUseCase<RappiWebhookEventOrderRequest>, RappiUpdateOrderStatusUseCase>();

        return services
                .Configure<JsonOptions>(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
            ;
    }

    private static IServiceCollection AddRappiClients(this IServiceCollection services)
    {
        string baseUrl = AppEnv.INTEGRATIONS.RAPPI.ENDPOINT.BASE_URL.NotNull();
        string baseAuthUrl = AppEnv.INTEGRATIONS.RAPPI.ENDPOINT.AUTH.NotNull();

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