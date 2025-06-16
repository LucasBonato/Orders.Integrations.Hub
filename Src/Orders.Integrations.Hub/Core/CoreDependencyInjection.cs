using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SimpleNotificationService;

using .AWS.Credentials;
using Orders.Integrations.Hub.Core.Adapter;
using Orders.Integrations.Hub.Core.Application.Clients;
using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Application.Middlewares;
using Orders.Integrations.Hub.Core.Application.UseCases;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases;

using FastEndpoints;

using Microsoft.AspNetCore.Mvc;

using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Orders.Integrations.Hub.Core;

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
        app.UseExceptionHandler(_ => { });
        app.UseFastEndpoints();
        app.AddOrdersHubEndpoints();
        return app;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<ExceptionHandlerMiddleware>();

        services.AddTransient<IAmazonSimpleNotificationService>(_ => SimplesNotificationServiceConfiguration());
        services.AddTransient<IObjectStorageClient, SimpleStorageServiceClient>();

        services.AddScoped<IOrderUseCase, OrderUseCase>();
        services.AddScoped<IOrderDisputeUpdateUseCase, OrderDisputeUpdateUseCase>();

        services.AddFastEndpoints(options => {
            options.DisableAutoDiscovery = false;
        });

        services.AddSingleton<ICustomJsonSerializer, CoreJsonSerializer>();
        services.Configure<JsonOptions>(options => {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
        });
        return services;
    }

    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpClient<IInternalClient, InternalClient>(client => {
            client.BaseAddress = new Uri(AppEnv..MONOLITH.ENDPOINT.BASE_URL.NotNullEnv());
        });

        services.AddHttpClient<IOrderClient, OrderClient>(client => {
            client.BaseAddress = new Uri(AppEnv..ORDERS.ENDPOINT.BASE_URL.NotNullEnv());
        });

        services.AddHttpClient<IOrderHttp>(client => {
            client.BaseAddress = new Uri(AppEnv..ORDERS.ENDPOINT.BASE_URL.NotNullEnv());
        });

        return services;
    }

    private static IServiceCollection AddOpenTelemetryConfiguration(this IServiceCollection services)
    {
        string serviceName = AppEnv.OTEL_SERVICE_NAME.NotNullEnv();

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

        profile = AppEnv.AWS_PROFILE.NotNullEnv();
        return new AmazonSimpleNotificationServiceClient(SSOCredentials.LoadSsoCredentials(profile));
    }
}