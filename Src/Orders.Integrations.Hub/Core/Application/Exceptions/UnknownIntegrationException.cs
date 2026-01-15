using System.Net;

using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Application.Integration;
using Orders.Integrations.Hub.Core.Domain.Contracts;

namespace Orders.Integrations.Hub.Core.Application.Exceptions;

public class UnknownIntegrationException(
    Type useCaseType,
    IntegrationKey key
) : Exception($"The integration of type <{useCaseType}> using the key <{key}> is unknown!"), IProblemDetailsException {
    private static HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;

    public ProblemDetails ToProblemDetails() {
        return new ProblemDetails {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2",
            Title = "The integration is unknown",
            Status = (int)StatusCode,
            Detail = Message,
        };
    }
}