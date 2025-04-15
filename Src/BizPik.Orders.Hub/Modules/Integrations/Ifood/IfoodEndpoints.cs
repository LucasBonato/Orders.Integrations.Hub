using BizPik.Orders.Hub.Modules.Integrations.Common.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Clients;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Handlers;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Validators;

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

        routeGroup.MapPost("/webhook", IfoodAdapter.Webhook).AddEndpointFilter<IfoodSignatureValidator>();

        return app;
    }

    public static IServiceCollection AddIfood(this IServiceCollection services)
        => services
            .AddIfoodServices()
            .AddIfoodClients()
        ;

    private static IServiceCollection AddIfoodServices(this IServiceCollection services)
    {
        services.AddScoped<IfoodAuthMessageHandler>();
        return services;
    }

    private static IServiceCollection AddIfoodClients(this IServiceCollection services)
    {
        services.AddHttpClient<IIntegrationAuthClient<IfoodAuthTokenRequest, IfoodAuthTokenResponse>, IfoodAuthClient>(client => {
            client.BaseAddress = new Uri(AppEnv.IFOOD.BASE_URL.NotNull());
        });

        services.AddHttpClient<IIFoodClient, IfoodClient>(client => {
            client.BaseAddress = new Uri(AppEnv.IFOOD.BASE_URL.NotNull());
        }).AddHttpMessageHandler<IfoodAuthMessageHandler>();

        return services;
    }
}