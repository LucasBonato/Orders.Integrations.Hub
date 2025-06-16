using Orders.Integrations.Hub.Core.Application.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace Orders.Integrations.Hub.Core.Application.Middlewares.Handlers;

public static class EnvironmentVariableNotFoundExceptionHandler
{
    public static ProblemDetails HandleException(this EnvironmentVariableNotFoundException exception) {
        return new ProblemDetails {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2",
            Title = "The environment variable was not found",
            Status = (int)EnvironmentVariableNotFoundException.StatusCode,
            Detail = exception.Message,
        };
    }
}