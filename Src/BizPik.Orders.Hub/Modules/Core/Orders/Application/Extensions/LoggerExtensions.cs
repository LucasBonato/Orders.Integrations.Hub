using System.Text.Encodings.Web;
using System.Text.Json;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.Extensions;

public static class LoggerExtensions
{
    public static void LogStructuredException(this ILogger logger, Exception ex, HttpContext context, string traceId, string traceParent)
    {
        using (logger.BeginScope(new Dictionary<string, object> { ["ExceptionType"] = ex.GetType().FullName! }))
        {
            var message = new {
                TraceId = traceId,
                TraceParent = traceParent,
                context.Request.Path,
                Type = ex.GetType().Name,
                ex.Message,
                ex.StackTrace,
            };

            string json = JsonSerializer.Serialize(message, new JsonSerializerOptions {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            });

            string formattedJson = json.Replace(@"\r\n", Environment.NewLine);


            logger.LogError("{Body}", formattedJson);
        }
    }
}