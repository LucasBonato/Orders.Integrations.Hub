using System.Diagnostics;
using System.Net;

using Orders.Integrations.Hub.Core.Infrastructure.Middlewares;

namespace Orders.Integrations.Hub.Core.Infrastructure.Extensions;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddProblemDetailsConfiguration(this IServiceCollection services)
    {
        services
            .AddProblemDetails(options =>
                options.CustomizeProblemDetails = context => {
                    HttpContext httpContext = context.HttpContext;
                    string traceId = Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier;
                    string traceParent = Activity.Current?.Id ?? httpContext.TraceIdentifier;

                    var logger = httpContext.RequestServices.GetRequiredService<ILogger<ExceptionHandlerMiddleware>>();
                        
                    if (context.Exception is not null)
                        logger.LogStructuredException(
                            context.Exception,
                            httpContext,
                            traceId,
                            traceParent
                        );

                    if (string.IsNullOrEmpty(context.ProblemDetails.Type))
                        context.ProblemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

                    context.ProblemDetails.Instance = httpContext.Request.Path;
                    context.ProblemDetails.Extensions.TryAdd("method", httpContext.Request.Method);

                    if (context.ProblemDetails.Extensions.ContainsKey("traceId"))
                        context.ProblemDetails.Extensions["traceId"] = traceId;
                    else
                        context.ProblemDetails.Extensions.TryAdd("traceId", traceId);

                    httpContext.Response.StatusCode = context.ProblemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.Headers.TryAdd("traceparent", traceParent);
                }
            )
            ;
        
        return services;
    } 
}