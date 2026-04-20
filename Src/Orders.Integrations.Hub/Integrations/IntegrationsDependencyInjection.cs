using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.Food99;
using Orders.Integrations.Hub.Integrations.IFood;
using Orders.Integrations.Hub.Integrations.Rappi;

namespace Orders.Integrations.Hub.Integrations;

public static class IntegrationsDependencyInjection
{
    public static IServiceCollection AddIntegrationsModule(this IServiceCollection services)
    {
        return services
                .AddServices()
                .AddIFood()
                .AddRappi()
                .AddFood99()
            ;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
                .AddScoped<IIntegrationContext, IntegrationContext>()
                .AddSingleton<HmacSha256SignatureValidator>()
                .AddSingleton<Md5SignatureValidator>()
            ;
    }
}