using System.Diagnostics;
using System.Net;

using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Core.Application.Middlewares.Handlers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Domain.Contracts;

namespace Orders.Integrations.Hub.Core.Application.Middlewares;

public class ExceptionHandlerMiddleware(
    ILogger<ExceptionHandlerMiddleware> logger
) : IExceptionHandler {
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        string traceId = Activity.Current?.TraceId.ToString()?? httpContext.TraceIdentifier;
        string traceParent = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        logger.LogStructuredException(exception, httpContext, traceId, traceParent);

        ProblemDetails details = exception is IProblemDetailsException e
            ? e.ToProblemDetails()
            : exception switch {
                InvalidOperationException ex => ex.HandleException(),
                ArgumentNullException ex => ex.HandleException(),
                _ => exception.HandleException()
            };

        if (string.IsNullOrEmpty(details.Type)) {
            details.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        }

        details.Instance = httpContext.Request.Path;
        details.Extensions.TryAdd("method", httpContext.Request.Method);
        details.Extensions.TryAdd("traceId", traceId);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = details.Status?? (int)HttpStatusCode.InternalServerError;

        httpContext.Response.Headers.TryAdd("traceparent", traceParent);

        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

        return true;
    }
}