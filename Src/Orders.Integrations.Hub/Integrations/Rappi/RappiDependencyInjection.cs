using Microsoft.Extensions.Options;

using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Integrations.Common.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Serialization;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Clients;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.In;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.Out;
using Orders.Integrations.Hub.Integrations.Rappi.Application.ValueObjects;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Infrastructure;
using Orders.Integrations.Hub.Integrations.Rappi.Infrastructure.Options;

using Refit;

namespace Orders.Integrations.Hub.Integrations.Rappi;

public static class RappiDependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRappi(IConfiguration configuration)
            => services
                .AddRappiServices()
                .AddRappiClients(configuration)
        ;

        private IServiceCollection AddRappiServices()
        {
            services.AddTransient<IOrderCreateUseCase<RappiOrder>, RappiOrderCreateUseCase>();
            services.AddTransient<IOrderUpdateUseCase<RappiWebhookEventOrderRequest>, RappiOrderUpdateUseCase>();

            services.AddScoped<RappiSignatureValidator>();
            services.AddScoped<IWebhookSignatureValidator, RappiSignatureValidator>();
            services.AddScoped<RappiOrderResolver>();
            services.AddScoped<IWebhookSignatureResolver<RappiOrder>, RappiOrderResolver>();
            services.AddScoped<RappiOrderEventResolver>();
            services.AddScoped<IWebhookSignatureResolver<RappiWebhookEventOrderRequest>, RappiOrderEventResolver>();
            services.AddScoped<RappiPingResolver>();
            services.AddScoped<IWebhookSignatureResolver<RappiWebhookPingRequest>, RappiPingResolver>();

            services.AddKeyedScoped<IOrderChangeStatusUseCase, RappiOrderChangeStatusUseCase>(RappiIntegrationKey.Value);
            services.AddKeyedScoped<IOrderChangeProductStatusUseCase, RappiOrderChangeProductStatusUseCase>(RappiIntegrationKey.Value);
            services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, RappiOrderGetCancellationReasonUseCase>(RappiIntegrationKey.Value);

            services.AddKeyedSingleton<ICustomJsonSerializer, RappiJsonSerializer>(RappiIntegrationKey.Value);

            return services;
        }

        private IServiceCollection AddRappiClients(IConfiguration configuration)
        {
            services.AddOptions<RappiOptions>()
                .Bind(configuration.GetSection(RappiOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddHttpClient<IRappiAuthClient, RappiAuthClient>((serviceProvider, client) => {
                RappiOptions options = serviceProvider.GetRequiredService<IOptions<RappiOptions>>().Value;
                client.BaseAddress = options.Endpoint.AuthUrl;
            });

            services.AddScoped<RappiAuthMessageHandler>();

            services.AddRefitClient<IRappiClient>(serviceProvider => new RefitSettings {
                    ContentSerializer = new CustomJsonContentSerializer(
                        serviceProvider.GetRequiredKeyedService<ICustomJsonSerializer>(RappiIntegrationKey.Value)
                    )
                })
                .ConfigureHttpClient((serviceProvider, client) => {
                    RappiOptions options = serviceProvider.GetRequiredService<IOptions<RappiOptions>>().Value;
                    client.BaseAddress = options.Endpoint.BaseUrl;
                })
                .AddHttpMessageHandler<IntegrationContextHandler>()
                .AddHttpMessageHandler<RappiAuthMessageHandler>();

            return services;
        }
    }
}