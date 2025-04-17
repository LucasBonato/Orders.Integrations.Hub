using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SimpleNotificationService;
using BizPik.AWS.Credentials;
using BizPik.Orders.Hub.Modules.Core.BizPik.Application;
using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.Orders;
using BizPik.Orders.Hub.Modules.Core.Orders.Application.Clients;
using BizPik.Orders.Hub.Modules.Core.Orders.Application.UseCases;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Core.Orders.Domain.Contracts.UseCases;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Ports;

using FastEndpoints;

using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BizPik.Orders.Hub.Modules.Core;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services
                .AddOpenTelemetryConfiguration()
                .AddServices()
                .AddClients()
            ;
    }

    public static IApplicationBuilder UseCore(this WebApplication app)
    {
        app.UseFastEndpoints();
        app.AddOrdersHubEndpoints();
        return app;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IOrderChangeStatusUseCase, IfoodOrderChangeStatusUseCase>();
        services.AddScoped<IOrderUseCase, OrderUseCase>();
        services.AddTransient<IAmazonSimpleNotificationService>(_ => SimplesNotificationServiceConfiguration());
        services.AddFastEndpoints(options => {
            options.DisableAutoDiscovery = false;
        });
        return services;
    }

    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpClient<IBizPikMonolithClient, BizPikMonolithClient>(client => {
            client.BaseAddress = new Uri(AppEnv.BIZPIK.MONOLITH.ENDPOINT.BASE_URL.NotNull());
        });

        services.AddHttpClient<IOrderClient, OrderClient>(client => {
            client.BaseAddress = new Uri(AppEnv.BIZPIK.ORDERS.ENDPOINT.BASE_URL.NotNull());
        });

        services.AddHttpClient<IOrderHttp>(client => {
            client.BaseAddress = new Uri(AppEnv.BIZPIK.ORDERS.ENDPOINT.BASE_URL.NotNull());
        });

        return services;
    }

    private static IServiceCollection AddOpenTelemetryConfiguration(this IServiceCollection services)
    {
        string serviceName = AppEnv.OTEL_SERVICE_NAME.NotNull();

        services
            .AddOpenTelemetry()
            .UseOtlpExporter()
            .ConfigureResource(resource => {
                resource.AddService(serviceName: serviceName);
            })
            .WithTracing(tracing => {
                tracing
                    .AddSource(serviceName)
                    .AddAspNetCoreInstrumentation()
                    .AddAWSInstrumentation()
                    .AddHttpClientInstrumentation()
                ;
            })

        ;

        services.AddLogging(options => {
            options
                // .SetMinimumLevel(LogLevel.Information)
                // .AddFilter("System.Net.Http.HttpClient.OtlpTraceExporter", LogLevel.None)
                // .AddFilter("System.Net.Http.HttpClient.OtlpMetricExporter", LogLevel.None)
                .AddOpenTelemetry(logger => {
                    logger.IncludeScopes = true;
                    logger.ParseStateValues = true;
                    logger.IncludeFormattedMessage = true;
                })
            ;
        });

        return services;
    }

    private static AmazonSimpleNotificationServiceClient SimplesNotificationServiceConfiguration()
    {
        bool isLocalSns = AppEnv.PUB_SUB.TOPICS.IS_LOCAL.GetDefault(false);
        string profile;

        if (isLocalSns) {
            AmazonSimpleNotificationServiceConfig config = new();
            profile = "localstack";
            new CredentialProfileStoreChain().TryGetAWSCredentials(profile, out AWSCredentials? credentials);
            config.Profile = new Profile(profile);
            config.ServiceURL = "http://localhost:4566";
            return new AmazonSimpleNotificationServiceClient(credentials: credentials, clientConfig: config);
        }

        profile = AppEnv.AWS_PROFILE.NotNull();
        return new AmazonSimpleNotificationServiceClient(SSOCredentials.LoadSsoCredentials(profile));
    }
}