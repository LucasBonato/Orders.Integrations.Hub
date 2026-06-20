using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.S3;
using Amazon.SimpleNotificationService;

using MassTransit;

using Microsoft.AspNetCore.Mvc;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;
using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Distributed;
using Orders.Integrations.Hub.Core.Adapters.Out.Cache.Hybrid;
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
    public static WebApplication UseCore(this WebApplication app)
    {
        app.MapEndpoints();
        app.UseExceptionHandler(_ => { });
        return app;
    }

    extension(IServiceCollection services)
    {
        public IServiceCollection AddCore()
        {
            return services
                    .AddOpenTelemetryConfiguration()
                    .AddServices()
                    .AddClients()
                ;
        }
        
        private IServiceCollection AddServices()
        {
            services.AddEndpoints(Assembly.GetExecutingAssembly());
            
            services.AddExceptionHandler<ExceptionHandlerMiddleware>();

            services.AddSingleton<IAmazonSimpleNotificationService>(_ => AwsConfigurationExtensions.SimpleNotificationServiceConfiguration());
            services.AddSingleton<IAmazonS3>(_ => AwsConfigurationExtensions.SimpleStorageServiceConfiguration());

            services.AddSingleton<IObjectStorageClient, SimpleStorageServiceClient>();

            services.AddScoped<IIntegrationRouter, IntegrationRouter>();
            
            services
                .AddMessageBroker()
                .AddCacheServices()
                .AddProblemDetails(options =>
                    options.CustomizeProblemDetails = context => {
                        HttpContext httpContext = context.HttpContext;
                        string traceId = Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier;
                        string traceParent = Activity.Current?.Id ?? httpContext.TraceIdentifier;

                        var logger = httpContext.RequestServices.GetRequiredService<ILogger<ExceptionHandlerMiddleware>>();
                        
                        if (context.Exception is not null)
                            logger.LogStructuredException(
                                context.Exception,
                                httpContext,
                                traceId,
                                traceParent
                            );

                        if (string.IsNullOrEmpty(context.ProblemDetails.Type))
                            context.ProblemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

                        context.ProblemDetails.Instance = httpContext.Request.Path;
                        context.ProblemDetails.Extensions.TryAdd("method", httpContext.Request.Method);

                        if (context.ProblemDetails.Extensions.ContainsKey("traceId"))
                            context.ProblemDetails.Extensions["traceId"] = traceId;
                        else
                            context.ProblemDetails.Extensions.TryAdd("traceId", traceId);

                        httpContext.Response.StatusCode = context.ProblemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
                        httpContext.Response.Headers.TryAdd("traceparent", traceParent);
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
                        .AddSource(
                            serviceName,
                            nameof(MassTransit)
                        )
                        .AddAspNetCoreInstrumentation()
                        .AddAWSInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRedisInstrumentation()
                        ;
                })
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddMeter(
                            serviceName,
                            nameof(MassTransit)
                        )
                        .AddAspNetCoreInstrumentation()
                        .AddAWSInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddView(instrument =>
                            instrument.GetType().GetGenericTypeDefinition() == typeof(Histogram<>)
                                ? new ExplicitBucketHistogramConfiguration()
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
            string cacheMode = AppEnv.CACHE.MODE.NotNullEnv();

            return cacheMode switch {
                "Memory" => services
                    .AddMemoryCache()
                    .AddSingleton<ICacheService, MemoryCacheService>(),
                
                "Distributed" => services
                    .AddStackExchangeRedisCache(options => {
                        options.Configuration = AppEnv.CACHE.CONFIGURATIONS.CONNECTION_STRING.NotNullEnv();
                        options.InstanceName = "redis-only-instance";
                    })
                    .AddSingleton<ICacheService, RedisCacheService>(),
                
                "Hybrid" => services
                    .AddMemoryCache()
                    .AddStackExchangeRedisCache(options => {
                        options.Configuration = AppEnv.CACHE.CONFIGURATIONS.CONNECTION_STRING.NotNullEnv();
                        options.InstanceName = "redis-hybrid-instance";
                    })
                    .AddHybridCache()
                    .Services
                    .AddSingleton<ICacheService, HybridCacheService>(),
                
                _ => throw new InvalidOperationException("Invalid cache mode!")
            };
        }

        private IServiceCollection AddMessageBroker() {
            
            string brokerMode = AppEnv.MESSAGE_BROKER.MODE.NotNullEnv();
            
            services.AddSingleton<ICommandDispatcher, MassTransitCommandDispatcher>();
            
            services.AddMassTransit(busConfigurator => {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumer<UpdateOrderCommandHandler>();
                busConfigurator.AddConsumer<CreateOrderCommandHandler>();
                busConfigurator.AddConsumer<PubSubCommandHandler>();
                busConfigurator.AddConsumer<ProcessOrderDisputeCommandHandler>();

                if (brokerMode == "Memory") {
                    busConfigurator.UsingInMemory((context, configurator) => {
                        configurator.UseMessageRetry(retry => retry.Interval(5, TimeSpan.FromSeconds(5)));
                        configurator.UseInMemoryOutbox(context);
                        configurator.ConfigureJsonSerializerOptions(options => {
                            options.Converters.Add(new IntegrationKeyJsonConverter());
                            return options;
                        });
                        configurator.ConfigureEndpoints(context);
                    });
                    
                    return;
                }

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(AppEnv.MESSAGE_BROKER.CONFIGURATIONS.CONNECTION_STRING.NotNullEnv());
                    
                    configurator.UseCircuitBreaker(circuitBreaker => {
                        circuitBreaker.TrackingPeriod = TimeSpan.FromMinutes(1);
                        circuitBreaker.TripThreshold = 15;
                        circuitBreaker.ActiveThreshold = 10;
                        circuitBreaker.ResetInterval = TimeSpan.FromMinutes(5);
                    });
                    
                    configurator.UseMessageRetry(retry => {
                        retry.Exponential(
                            retryLimit: 5, 
                            minInterval: TimeSpan.FromSeconds(1), 
                            maxInterval: TimeSpan.FromMinutes(2), 
                            intervalDelta: TimeSpan.FromSeconds(5)
                        );
                    });
                    
                    configurator.UseInMemoryOutbox(context);
                    configurator.ConfigureJsonSerializerOptions(options => {
                        options.Converters.Add(new IntegrationKeyJsonConverter());
                        return options;
                    });
                    configurator.ConfigureEndpoints(context);
                });

            });
            
            return services;
        }
    }
}