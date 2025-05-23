using System.Net;

namespace BizPik.Orders.Hub.Core.Orders.Application.Exceptions;

public class EnvironmentVariableNotFoundException(string name) : Exception($"The environment variable <{name}> was not found!") {
    public static HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
}