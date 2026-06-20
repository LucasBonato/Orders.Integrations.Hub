using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Common.Application.Handlers;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.Food99;
using Orders.Integrations.Hub.Integrations.IFood;
using Orders.Integrations.Hub.Integrations.Rappi;

namespace Orders.Integrations.Hub.Integrations;

public static class IntegrationsDependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddIntegrationsModule()
        {
            return services
                    .AddServices()
                    .AddIFood()
                    .AddRappi()
                    .AddFood99()
                ;
        }

        private IServiceCollection AddServices()
        {
            return services
                    .AddHttpContextAccessor()
                    .AddScoped<IIntegrationContext, IntegrationContext>()
                    .AddScoped<IntegrationContextHandler>()
                    .AddSingleton<HmacSha256SignatureValidator>()
                    .AddSingleton<Md5SignatureValidator>()
                ;
        }
    }
}
