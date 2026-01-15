using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.S3;
using Amazon.SimpleNotificationService;

using FastEndpoints;

using Microsoft.AspNetCore.Mvc;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Orders.Integrations.Hub.Core.Application.Clients;
using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Application.Integration;
using Orders.Integrations.Hub.Core.Application.Middlewares;
using Orders.Integrations.Hub.Core.Application.Services;
using Orders.Integrations.Hub.Core.Application.UseCases;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.Contracts.Clients;
using Orders.Integrations.Hub.Core.Domain.Contracts.UseCases.Core;
using Orders.Integrations.Hub.Core.Infrastructure;

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

    public static WebApplication UseCore(this WebApplication app)
    {
        app.UseExceptionHandler(_ => { });
        app.UseFastEndpoints();
        app.AddOrdersHubEndpoints();
        return app;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<ExceptionHandlerMiddleware>();

        services.AddSingleton<IAmazonSimpleNotificationService>(_ => AwsConfigurationExtensions.SimpleNotificationServiceConfiguration());
        services.AddSingleton<IAmazonS3>(_ => AwsConfigurationExtensions.SimpleStorageServiceConfiguration());

        services.AddSingleton<IObjectStorageClient, SimpleStorageServiceClient>();

        services.AddScoped<IOrderUseCase, OrderUseCase>();
        services.AddScoped<IOrderDisputeUpdateUseCase, OrderDisputeUpdateUseCase>();

        services.AddFastEndpoints(options => {
            options.DisableAutoDiscovery = false;
        });

        services
            .AddCacheServices()
            .AddSingleton<ICacheService, MemoryCacheService>()
            .AddProblemDetails(options =>
                options.CustomizeProblemDetails = context => {
                    HttpContext httpContext = context.HttpContext;
                    string traceId = Activity.Current?.TraceId.ToString()?? httpContext.TraceIdentifier;
                    string traceParent = Activity.Current?.Id ?? httpContext.TraceIdentifier;

                    var logger = httpContext.RequestServices.GetRequiredService<ILogger<ExceptionHandlerMiddleware>>();
                    if (context.Exception is not null)
                        logger.LogStructuredException(context.Exception, httpContext, traceId, traceParent);

                    if (string.IsNullOrEmpty(context.ProblemDetails.Type)) {
                        context.ProblemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                    }

                    context.ProblemDetails.Instance = httpContext.Request.Path;
                    context.ProblemDetails.Extensions.TryAdd("method", httpContext.Request.Method);

                    if (context.ProblemDetails.Extensions.ContainsKey("traceId"))
                        context.ProblemDetails.Extensions["traceId"] = traceId;
                    else
                        context.ProblemDetails.Extensions.TryAdd("traceId", traceId);

                    httpContext.Response.StatusCode = context.ProblemDetails.Status?? (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.Headers.TryAdd("traceparent", traceParent);
                }
            )
            ;

        services.AddSingleton<ICustomJsonSerializer, CoreJsonSerializer>();
        services.Configure<JsonOptions>(options => {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
        });
        services.ConfigureHttpJsonOptions(options => {
            options.SerializerOptions.Converters.Add(new IntegrationKeyJsonConverter());
        });
        return services;
    }

    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpClient<IInternalClient, InternalClient>(client => {
            client.BaseAddress = new Uri(AppEnv.INTERNAL.ENDPOINT.BASE_URL.NotNullEnv());
        });
        services.Decorate<IInternalClient, InternalCacheClient>();

        services.AddHttpClient<IOrderClient, OrderClient>(client => {
            client.BaseAddress = new Uri(AppEnv.ORDERS.ENDPOINT.BASE_URL.NotNullEnv());
        });

        services.AddHttpClient<IOrderHttp>(client => {
            client.BaseAddress = new Uri(AppEnv.ORDERS.ENDPOINT.BASE_URL.NotNullEnv());
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
            .WithMetrics(metrics =>
            {
                metrics
                    .AddMeter(serviceName)
                    .AddAspNetCoreInstrumentation()
                    .AddAWSInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddView(instrument =>
                        instrument.GetType().GetGenericTypeDefinition() == typeof(Histogram<>)
                            ? new Base2ExponentialBucketHistogramConfiguration()
                            : null
                    )
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

    private static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        return services
                .AddMemoryCache()
            ;
    }
}