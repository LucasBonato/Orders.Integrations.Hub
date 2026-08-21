using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration) {
        return services
                .AddProblemDetailsConfiguration()
                .AddApiVersioningConfiguration()
                .AddObservabilityConfiguration(configuration)
                .AddMessageBrokerConfiguration(configuration)
                .AddSerializationConfiguration()
                .AddCoreSpecificConfiguration()
                .AddClientsConfiguration(configuration)
                .AddCacheConfiguration(configuration)
                .AddAwsConfiguration(configuration)
            ;
    }
}