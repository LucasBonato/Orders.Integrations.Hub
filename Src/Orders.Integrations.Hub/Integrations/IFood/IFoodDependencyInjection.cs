using Microsoft.Extensions.Options;

using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Integrations.Common.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Serialization;
using Orders.Integrations.Hub.Integrations.IFood.Application.Clients;
using Orders.Integrations.Hub.Integrations.IFood.Application.Handlers;
using Orders.Integrations.Hub.Integrations.IFood.Application.Ports.In;
using Orders.Integrations.Hub.Integrations.IFood.Application.Ports.Out;
using Orders.Integrations.Hub.Integrations.IFood.Application.ValueObjects;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Infrastructure;
using Orders.Integrations.Hub.Integrations.IFood.Infrastructure.Options;

using Refit;

namespace Orders.Integrations.Hub.Integrations.IFood;

public static class IFoodDependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIFood(IConfiguration configuration)
            => services
                .AddIFoodServices()
                .AddIFoodClients(configuration)
        ;

        private IServiceCollection AddIFoodServices()
        {
            services.AddTransient<IOrderCreateUseCase<IFoodWebhookRequest>, IFoodOrderCreateUseCase>();
            services.AddTransient<IOrderUpdateUseCase<IFoodWebhookRequest>, IFoodOrderUpdateUseCase>();
            services.AddTransient<IOrderDisputeUseCase<IFoodWebhookRequest>, IFoodHandshakeOrderDisputeUseCase>();

            services.AddScoped<IFoodSignatureStrategy>();

            services.AddKeyedScoped<IOrderChangeStatusUseCase, IFoodOrderChangeStatusUseCase>(IFoodIntegrationKey.Value);
            services.AddKeyedScoped<IOrderDisputeRespondUseCase, IFoodHandshakeOrderDisputeRespondUseCase>(IFoodIntegrationKey.Value);
            services.AddKeyedScoped<IOrderChangeProductStatusUseCase, IFoodOrderChangeProductStatusUseCase>(IFoodIntegrationKey.Value);
            services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, IFoodOrderGetCancellationReasonUseCase>(IFoodIntegrationKey.Value);

            services.AddKeyedScoped<IOrderDisputeEvidenceStorage<Media>, IFoodDisputeEvidenceStorage>(IFoodIntegrationKey.Value);

            services.AddKeyedSingleton<ICustomJsonSerializer, CommonJsonSerializer>(IFoodIntegrationKey.Value);

            return services;
        }

        private IServiceCollection AddIFoodClients(IConfiguration configuration)
        {
            services.AddOptions<IFoodOptions>()
                .Bind(configuration.GetSection(IFoodOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddHttpClient<IIFoodAuthClient, IFoodAuthClient>((serviceProvider, client) => {
                IFoodOptions options = serviceProvider.GetRequiredService<IOptions<IFoodOptions>>().Value;
                client.BaseAddress = options.Endpoint.BaseUrl;
            });

            services.AddScoped<IFoodAuthMessageHandler>();

            services.AddRefitClient<IIFoodClient>(serviceProvider => new RefitSettings {
                    ContentSerializer = new CustomJsonContentSerializer(
                        serviceProvider.GetRequiredKeyedService<ICustomJsonSerializer>(IFoodIntegrationKey.Value)
                    )
                })
                .ConfigureHttpClient((serviceProvider, client) => {
                    IFoodOptions options = serviceProvider.GetRequiredService<IOptions<IFoodOptions>>().Value;
                    client.BaseAddress = options.Endpoint.BaseUrl;
                })
                .AddHttpMessageHandler<IntegrationContextHandler>()
                .AddHttpMessageHandler<IFoodAuthMessageHandler>();

            return services;
        }
    }
}