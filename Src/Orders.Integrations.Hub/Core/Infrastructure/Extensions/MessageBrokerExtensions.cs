using MassTransit;

using Orders.Integrations.Hub.Core.Adapters.In.Messaging.EventHandlers;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Messaging;
using Orders.Integrations.Hub.Core.Infrastructure.Messaging;
using Orders.Integrations.Hub.Core.Infrastructure.Options;
using Orders.Integrations.Hub.Core.Infrastructure.Serialization;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class MessageBrokerExtensions
{
    public static IServiceCollection AddMessageBrokerConfiguration(this IServiceCollection services, IConfiguration configuration) {
        MessageBrokerOptions messageBrokerOptions = configuration
            .GetSection(MessageBrokerOptions.SectionName)
            .Get<MessageBrokerOptions>()
                ?? throw new InvalidOperationException($"Missing '{MessageBrokerOptions.SectionName}' configuration section.");
        
        services.AddSingleton<ICommandDispatcher, MassTransitCommandDispatcher>();
        
        services.AddMassTransit(busConfigurator => {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<UpdateOrderCommandHandler>();
            busConfigurator.AddConsumer<CreateOrderCommandHandler>();
            busConfigurator.AddConsumer<PubSubCommandHandler>();
            busConfigurator.AddConsumer<ProcessOrderDisputeCommandHandler>();

            if (messageBrokerOptions.Provider == MessageBrokerProvider.Memory) {
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

            busConfigurator.UsingRabbitMq((context, configurator) => {
                string connectionString = configuration.GetConnectionString("RabbitMq") 
                    ?? throw new InvalidOperationException("Missing 'ConnectionStrings:RabbitMq' configuration value.");
                
                configurator.Host(connectionString);
                
                configurator.UseCircuitBreaker(circuitBreaker => {
                    circuitBreaker.TrackingPeriod = TimeSpan.FromMinutes(messageBrokerOptions.CircuitBreaker.TrackingPeriodMinutes);
                    circuitBreaker.TripThreshold = messageBrokerOptions.CircuitBreaker.TripThreshold;
                    circuitBreaker.ActiveThreshold = messageBrokerOptions.CircuitBreaker.ActiveThreshold;
                    circuitBreaker.ResetInterval = TimeSpan.FromMinutes(messageBrokerOptions.CircuitBreaker.ResetIntervalMinutes);
                });
                
                configurator.UseMessageRetry(retry => {
                    retry.Exponential(
                        retryLimit: messageBrokerOptions.Retry.RetryLimit,
                        minInterval: TimeSpan.FromSeconds(messageBrokerOptions.Retry.MinIntervalSeconds), 
                        maxInterval: TimeSpan.FromMinutes(messageBrokerOptions.Retry.MaxIntervalSeconds), 
                        intervalDelta: TimeSpan.FromSeconds(messageBrokerOptions.Retry.IntervalDeltaSeconds)
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