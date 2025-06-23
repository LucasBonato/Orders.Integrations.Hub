using Microsoft.AspNetCore.Mvc;

namespace BizPik.Orders.Hub.Core.Domain.Contracts;

public interface IProblemDetailsException {
    ProblemDetails ToProblemDetails();
}