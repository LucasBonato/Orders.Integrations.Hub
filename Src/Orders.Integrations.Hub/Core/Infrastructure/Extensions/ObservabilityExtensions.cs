using System.Diagnostics.Metrics;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class ObservabilityExtensions
{
    public static IServiceCollection AddObservabilityConfiguration(this IServiceCollection services)
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
            .WithMetrics(metrics => {
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