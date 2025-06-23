using System.Net;

using BizPik.Orders.Hub.Core.Domain.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace BizPik.Orders.Hub.Core.Application.Exceptions;

public class EnvironmentVariableNotFoundException(string name) : Exception($"The environment variable <{name}> was not found!"), IProblemDetailsException {
    public static HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    public ProblemDetails ToProblemDetails() {
        return new ProblemDetails {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2",
            Title = "The environment variable was not found",
            Status = (int)StatusCode,
            Detail = Message,
        };
    }
}