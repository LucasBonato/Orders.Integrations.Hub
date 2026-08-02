using MassTransit;

using Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;
using Orders.Integrations.Hub.Core.Infrastructure.Serialization;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class MessageBrokerExtensions
{
    public static IServiceCollection AddMessageBrokerConfiguration(this IServiceCollection services) {
        
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