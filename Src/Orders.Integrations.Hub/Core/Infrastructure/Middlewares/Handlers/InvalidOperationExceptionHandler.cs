using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace Orders.Integrations.Hub.Core.Infrastructure.Middlewares.Handlers;

public static class InvalidOperationExceptionHandler
{
    public static ProblemDetails HandleException(this InvalidOperationException exception) {
        return new ProblemDetails {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2",
            Title = "This Service don't have a implementation for this feature.",
            Status = (int)HttpStatusCode.NotImplemented,
            Detail = exception.Message,
        };
    }
}