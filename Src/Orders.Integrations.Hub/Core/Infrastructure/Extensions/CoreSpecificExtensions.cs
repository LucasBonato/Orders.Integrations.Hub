using System.Reflection;

using Orders.Integrations.Hub.Core.Application.Ports.In.Integration;
using Orders.Integrations.Hub.Core.Infrastructure.Integration;
using Orders.Integrations.Hub.Core.Infrastructure.Middlewares;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class CoreSpecificExtensions
{
    public static IServiceCollection AddCoreSpecificConfiguration(this IServiceCollection services) {
        services
            .AddEndpoints(Assembly.GetExecutingAssembly())
            .AddExceptionHandler<ExceptionHandlerMiddleware>()
            .AddScoped<IIntegrationRouter, IntegrationRouter>();
        
        return services;
    }
}