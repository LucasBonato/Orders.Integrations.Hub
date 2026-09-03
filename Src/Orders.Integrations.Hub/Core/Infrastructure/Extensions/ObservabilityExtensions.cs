using System.Diagnostics.Metrics;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Orders.Integrations.Hub.Core.Infrastructure.Options;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class ObservabilityExtensions
{
    public static IServiceCollection AddObservabilityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        OpenTelemetryOptions otlpOptions = configuration
            .GetSection(OpenTelemetryOptions.SectionName)
            .Get<OpenTelemetryOptions>() 
                ?? throw new InvalidOperationException($"Missing '{OpenTelemetryOptions.SectionName}' configuration section.");

        services
            .AddOpenTelemetry()
            .UseOtlpExporter(
                otlpOptions.Protocol,
                otlpOptions.Endpoint
            )
            .ConfigureResource(resource => {
                resource.AddService(serviceName: otlpOptions.ServiceName);
            })
            .WithTracing(tracing => {
                tracing
                    .AddSource(
                        otlpOptions.ServiceName,
                        nameof(MassTransit)
                    )
                    .AddAspNetCoreInstrumentation()
                    .AddAWSInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRedisInstrumentation()
                    ;
            })
            .WithMetrics(metrics => {
                metrics
                    .AddMeter(
                        otlpOptions.ServiceName,
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
}