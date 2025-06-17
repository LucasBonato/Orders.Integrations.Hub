using System.Text.Json;
using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Application.Extensions;
using BizPik.Orders.Hub.Core.Domain.Contracts;
using BizPik.Orders.Hub.Core.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;
using BizPik.Orders.Hub.Integrations.Rappi.Application.Clients;
using BizPik.Orders.Hub.Integrations.Rappi.Application.Handlers;
using BizPik.Orders.Hub.Integrations.Rappi.Application.Ports;
using BizPik.Orders.Hub.Integrations.Rappi.Application.Ports.In;
using BizPik.Orders.Hub.Integrations.Rappi.Application.Ports.Out;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.Contracts;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.Entity;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

using Microsoft.AspNetCore.Mvc;

namespace BizPik.Orders.Hub.Integrations.Rappi;

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

        services.AddKeyedScoped<IOrderChangeStatusUseCase, RappiOrderChangeStatusUseCase>(OrderIntegration.RAPPI);
        services.AddKeyedScoped<IOrderChangeProductStatusUseCase, RappiOrderChangeProductStatusUseCase>(OrderIntegration.RAPPI);
        services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, RappiOrderGetCancellationReasonUseCase>(OrderIntegration.RAPPI);

        services.AddSingleton<ICustomJsonSerializer, RappiJsonSerializer>();
        services.Configure<JsonOptions>(options => {
            options.JsonSerializerOptions.PropertyNamingPolicy = RappiJsonSerializer.Options.PropertyNamingPolicy;
            options.JsonSerializerOptions.DictionaryKeyPolicy = RappiJsonSerializer.Options.DictionaryKeyPolicy;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = RappiJsonSerializer.Options.PropertyNameCaseInsensitive;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
        });

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