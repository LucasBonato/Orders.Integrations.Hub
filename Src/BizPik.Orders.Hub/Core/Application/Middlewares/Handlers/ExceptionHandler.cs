using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace BizPik.Orders.Hub.Core.Application.Middlewares.Handlers;

public static class ExceptionHandler
{
    public static ProblemDetails HandleException(this Exception exception) {
        return new ProblemDetails() {
            Title = "An unexpected internal error occured.",
            Status = (int)HttpStatusCode.InternalServerError,
            Detail = exception.Message,
        };
    }
}