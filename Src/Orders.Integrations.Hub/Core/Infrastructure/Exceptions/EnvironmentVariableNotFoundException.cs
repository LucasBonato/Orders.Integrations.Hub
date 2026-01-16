using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace Orders.Integrations.Hub.Core.Infrastructure.Exceptions;

public class EnvironmentVariableNotFoundException(string name) : Exception($"The environment variable <{name}> was not found!"), IProblemDetailsException {
    private static HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    public ProblemDetails ToProblemDetails() {
        return new ProblemDetails {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2",
            Title = "The environment variable was not found",
            Status = (int)StatusCode,
            Detail = Message,
        };
    }
}