using Microsoft.AspNetCore.Mvc;

namespace Orders.Integrations.Hub.Core.Infrastructure.Exceptions;

public interface IProblemDetailsException {
    ProblemDetails ToProblemDetails();
}