using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Middlewares.Handlers;
using Orders.Integrations.Hub.Core.Domain.Contracts;

namespace Orders.Integrations.Hub.Core.Application.Middlewares;

public class ExceptionHandlerMiddleware(
    IProblemDetailsService problemDetailsService
) : IExceptionHandler {
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails details = exception is IProblemDetailsException e
            ? e.ToProblemDetails()
            : exception switch {
                InvalidOperationException ex => ex.HandleException(),
                ArgumentNullException ex => ex.HandleException(),
                _ => exception.HandleException()
            };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext {
            HttpContext = httpContext,
            ProblemDetails = details,
            Exception = exception
        });
    }
}