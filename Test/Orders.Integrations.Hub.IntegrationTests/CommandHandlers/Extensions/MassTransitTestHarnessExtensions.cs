using MassTransit;

using Microsoft.Extensions.DependencyInjection;

using Orders.Integrations.Hub.Core.Infrastructure.Serialization;

namespace Orders.Integrations.Hub.IntegrationTests.CommandHandlers.Extensions;

internal static class MassTransitTestHarnessExtensions {
    public static IServiceCollection AddDefaultTestHarness<TConsumer>(
        this IServiceCollection services
    ) where TConsumer : class, IConsumer =>
        services.AddMassTransitTestHarness(cfg => {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.AddConsumer<TConsumer>();
            cfg.UsingInMemory((context, configurator) => {
                configurator.ConfigureJsonSerializerOptions(options => {
                    options.Converters.Add(new IntegrationKeyJsonConverter());
                    return options;
                });
                configurator.ConfigureEndpoints(context);
            });
        });
}