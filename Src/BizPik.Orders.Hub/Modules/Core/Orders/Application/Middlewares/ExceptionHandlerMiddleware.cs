using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using BizPik.Orders.Hub.Modules.Core.Orders.Application.Extensions;
using BizPik.Orders.Hub.Modules.Core.Orders.Application.Middlewares.Handlers;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.Middlewares;

public class ExceptionHandlerMiddleware(
    ILogger<ExceptionHandlerMiddleware> logger
) : IExceptionHandler {
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        string traceId = httpContext.TraceIdentifier;

        logger.LogStructuredException(exception, httpContext, traceId);

        ProblemDetails details = exception switch {
            InvalidOperationException e => e.HandleException(),
            ArgumentNullException e => e.HandleException(),
            _ => exception.HandleException()
        };

        if (string.IsNullOrEmpty(details.Type)) {
            details.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        }

        details.Instance = httpContext.Request.Path;
        details.Extensions.Add("traceId", traceId);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = details.Status?? (int)HttpStatusCode.InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

        return true;
    }
}