using Microsoft.AspNetCore.Mvc;

namespace Orders.Integrations.Hub.Core.Domain.Contracts;

public interface IProblemDetailsException {
    ProblemDetails ToProblemDetails();
}