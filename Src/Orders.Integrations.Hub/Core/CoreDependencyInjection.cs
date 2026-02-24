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

using Orders.Integrations.Hub.Core.Adapters.In.Http;
using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Memory;
using Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;
using Orders.Integrations.Hub.Core.Application.Ports.In.Integration;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;
using Orders.Integrations.Hub.Core.Infrastructure.Integration;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;
using Orders.Integrations.Hub.Core.Infrastructure.Middlewares;
using Orders.Integrations.Hub.Core.Infrastructure.Serialization;

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

    extension(IServiceCollection services)
    {
        private IServiceCollection AddServices()
        {
            services.AddExceptionHandler<ExceptionHandlerMiddleware>();

            services.AddSingleton<IAmazonSimpleNotificationService>(_ =>
                AwsConfigurationExtensions.SimpleNotificationServiceConfiguration());
            services.AddSingleton<IAmazonS3>(_ => AwsConfigurationExtensions.SimpleStorageServiceConfiguration());

            services.AddSingleton<IObjectStorageClient, SimpleStorageServiceClient>();

            services.AddSingleton<ICommandDispatcher, FastEndpointsCommandDispatcher>();

            services.AddScoped<IIntegrationRouter, IntegrationRouter>();

            services.AddFastEndpoints(options =>
            {
                options.DisableAutoDiscovery = false;
            });

            services
                .AddCacheServices()
                .AddProblemDetails(options =>
                    options.CustomizeProblemDetails = context =>
                    {
                        HttpContext httpContext = context.HttpContext;
                        string traceId = Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier;
                        string traceParent = Activity.Current?.Id ?? httpContext.TraceIdentifier;

                        var logger = httpContext.RequestServices.GetRequiredService<ILogger<ExceptionHandlerMiddleware>>();
                        if (context.Exception is not null)
                            logger.LogStructuredException(context.Exception,
                                httpContext,
                                traceId,
                                traceParent);

                        if (string.IsNullOrEmpty(context.ProblemDetails.Type))
                        {
                            context.ProblemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                        }

                        context.ProblemDetails.Instance = httpContext.Request.Path;
                        context.ProblemDetails.Extensions.TryAdd("method",
                            httpContext.Request.Method);

                        if (context.ProblemDetails.Extensions.ContainsKey("traceId"))
                            context.ProblemDetails.Extensions["traceId"] = traceId;
                        else
                            context.ProblemDetails.Extensions.TryAdd("traceId",
                                traceId);

                        httpContext.Response.StatusCode = context.ProblemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
                        httpContext.Response.Headers.TryAdd("traceparent",
                            traceParent);
                    }
                )
                ;

            services.AddSingleton<ICustomJsonSerializer, CoreJsonSerializer>();
            services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
            });
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new IntegrationKeyJsonConverter());
            });
            return services;
        }

        private IServiceCollection AddClients()
        {
            services.AddHttpClient<IInternalClient, InternalClient>(client =>
            {
                client.BaseAddress = new Uri(AppEnv.INTERNAL.ENDPOINT.BASE_URL.NotNullEnv());
            });
            services.Decorate<IInternalClient, InternalCacheClient>();

            services.AddHttpClient<IOrderClient, OrderClient>(client =>
            {
                client.BaseAddress = new Uri(AppEnv.ORDERS.ENDPOINT.BASE_URL.NotNullEnv());
            });

            return services;
        }

        private IServiceCollection AddOpenTelemetryConfiguration()
        {
            string serviceName = AppEnv.OTEL_SERVICE_NAME.NotNullEnv();

            services
                .AddOpenTelemetry()
                .UseOtlpExporter()
                .ConfigureResource(resource =>
                {
                    resource.AddService(serviceName: serviceName);
                })
                .WithTracing(tracing =>
                {
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

            services.AddLogging(options =>
            {
                options
                    .AddOpenTelemetry(logger =>
                    {
                        logger.IncludeScopes = true;
                        logger.ParseStateValues = true;
                        logger.IncludeFormattedMessage = true;
                    })
                    ;
            });

            return services;
        }

        private IServiceCollection AddCacheServices()
        {
            return services
                    .AddMemoryCache()
                    .AddSingleton<ICacheService, MemoryCacheService>()
                ;
        }
    }
}