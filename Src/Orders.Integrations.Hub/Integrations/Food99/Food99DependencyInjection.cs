using Orders.Integrations.Hub.Core.Application.Ports.In.UseCases;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Application.Ports.Out.UseCases;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;
using Orders.Integrations.Hub.Integrations.Common.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Common.Serialization;
using Orders.Integrations.Hub.Integrations.Food99.Application.Clients;
using Orders.Integrations.Hub.Integrations.Food99.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Food99.Application.Ports.In;
using Orders.Integrations.Hub.Integrations.Food99.Application.Ports.Out;
using Orders.Integrations.Hub.Integrations.Food99.Application.ValueObjects;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Food99.Infrastructure;

using Refit;

namespace Orders.Integrations.Hub.Integrations.Food99;

public static class Food99DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddFood99()
            => services
                .AddFood99Services()
                .AddFood99Clients()
        ;

        private IServiceCollection AddFood99Services()
        {
            services.AddTransient<IOrderCreateUseCase<Food99WebhookRequest>, Food99OrderCreateUseCase>();
            services.AddTransient<IOrderUpdateUseCase<Food99WebhookRequest>, Food99OrderUpdateUseCase>();
            services.AddTransient<IOrderDisputeUseCase<Food99WebhookRequest>, Food99ApplyOrderDisputeUseCase>();

            services.AddScoped<Food99SignatureStrategy>();

            services.AddKeyedScoped<IOrderChangeStatusUseCase, Food99OrderChangeStatusUseCase>(Food99IntegrationKey.Value);
            services.AddKeyedScoped<IOrderGetCancellationReasonUseCase, Food99OrderGetCancellationReasonUseCase>(Food99IntegrationKey.Value);

            services.AddKeyedSingleton<ICustomJsonSerializer, Food99JsonSerializer>(Food99IntegrationKey.Value);

            return services;
        }

        private IServiceCollection AddFood99Clients()
        {
            string baseUrl = AppEnv.INTEGRATIONS.FOOD99.ENDPOINT.BASE_URL.NotNullEnv();

            services.AddHttpClient<IFood99AuthClient, Food99AuthClient>(client => {
                client.BaseAddress = new Uri(baseUrl);
            });

            services.AddScoped<Food99AuthMessageHandler>();

            services.AddRefitClient<IFood99Client>(serviceProvider => new RefitSettings {
                    ContentSerializer = new CustomJsonContentSerializer(
                        serviceProvider.GetRequiredKeyedService<ICustomJsonSerializer>(Food99IntegrationKey.Value)
                    )
                })
                .ConfigureHttpClient(client => {
                    client.BaseAddress = new Uri(baseUrl);
                })
                .AddHttpMessageHandler<IntegrationContextHandler>()
                .AddHttpMessageHandler<Food99AuthMessageHandler>();

            return services;
        }
    }
}
