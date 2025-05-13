namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.Extensions;

public static class LoggerExtensions
{
    public static void LogStructuredException(this ILogger logger, Exception ex, HttpContext context, string traceId)
    {
        using (logger.BeginScope(new Dictionary<string, object>
               {
                   ["traceId"] = traceId,
                   ["exceptionType"] = ex.GetType().FullName!,
                   ["requestPath"] = context.Request.Path
               }))
        {
            var message = new {
                TraceId = traceId,
                Path = context.Request.Path,
                Type = ex.GetType().Name,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
            };

            logger.LogError(
                new EventId(0, "ExceptionThrown"),
                "[ERROR] {body}",
                message
            );
        }
    }
}