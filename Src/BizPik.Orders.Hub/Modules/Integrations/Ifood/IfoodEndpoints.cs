using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Common.Application;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Clients;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Handlers;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Ports;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

using FastEndpoints;

using Microsoft.AspNetCore.Mvc;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood;

public static class IfoodEndpoints
{
    public static WebApplication AddIfoodEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeGroup = app
                .MapGroup("/ifood")
                .WithTags("Ifood")
                .WithDescription("Ifood Webhook Endpoint")
            ;

        routeGroup
            .MapPost("/webhook", IfoodAdapter.Webhook)
            // .AddEndpointFilter<IfoodSignatureValidator>()
        ;

        return app;
    }

    public static IServiceCollection AddIfood(this IServiceCollection services)
        => services
            .AddIfoodServices()
            .AddIfoodClients()
        ;

    private static IServiceCollection AddIfoodServices(this IServiceCollection services)
    {
        services.AddTransient<ICreateOrderUseCase<IfoodWebhookRequest>, IfoodCreateOrderUseCase>();
        services.AddTransient<IUpdateOrderStatusUseCase<IfoodWebhookRequest>, IfoodUpdateOrderStatusUseCase>();

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