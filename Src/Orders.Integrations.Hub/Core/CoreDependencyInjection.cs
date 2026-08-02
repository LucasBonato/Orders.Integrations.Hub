using Orders.Integrations.Hub.Core.Infrastructure.Extensions;

namespace Orders.Integrations.Hub.Core;

public static class CoreDependencyInjection
{
    public static WebApplication UseCore(this WebApplication app) {
        app
            .UseApiVersioningConfiguration()
            .UseExceptionHandler(_ => { });
        return app;
    }

    public static IServiceCollection AddCore(this IServiceCollection services) {
        return services
                .AddProblemDetailsConfiguration()
                .AddApiVersioningConfiguration()
                .AddObservabilityConfiguration()
                .AddMessageBrokerConfiguration()
                .AddSerializationConfiguration()
                .AddCoreSpecificConfiguration()
                .AddClientsConfiguration()
                .AddCacheConfiguration()
                .AddAwsConfiguration()
            ;
    }
}