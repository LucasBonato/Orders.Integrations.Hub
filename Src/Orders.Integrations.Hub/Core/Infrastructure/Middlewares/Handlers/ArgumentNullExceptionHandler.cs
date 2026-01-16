using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace Orders.Integrations.Hub.Core.Infrastructure.Middlewares.Handlers;

public static class ArgumentNullExceptionHandler
{
    public static ProblemDetails HandleException(this ArgumentNullException exception) {
        return new ProblemDetails() {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Title = "An ArgumentNullException occured.",
            Status = (int)HttpStatusCode.BadRequest,
            Detail = exception.Message,
        };
    }
}