using Asp.Versioning;
using Asp.Versioning.Builder;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class ApiVersioningExtensions
{
    public static WebApplication UseApiVersioningConfiguration(this WebApplication app) {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        
        app.MapEndpoints(
            app
                .MapGroup("/api/v{version:apiVersion}/orders-hub")
                .WithApiVersionSet(apiVersionSet)
        );

        return app;
    }
    
    public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options => {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            })
            .AddApiExplorer(options => {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddOpenApi();
        
        return services;
    }
}