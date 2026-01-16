using System.Net;

using Microsoft.AspNetCore.Mvc;

using Orders.Integrations.Hub.Core.Infrastructure.Integration;

namespace Orders.Integrations.Hub.Core.Infrastructure.Exceptions;

public class UnknownIntegrationException(
    IntegrationKey key
) : Exception($"The integration of type using the key <{key.Value}> is unknown!"), IProblemDetailsException {
    private static HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;

    public ProblemDetails ToProblemDetails() {
        return new ProblemDetails {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5",
            Title = "The integration is unknown",
            Status = (int)StatusCode,
            Detail = Message,
        };
    }
}