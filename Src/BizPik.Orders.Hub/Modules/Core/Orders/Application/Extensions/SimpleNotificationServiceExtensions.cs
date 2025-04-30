using System.Text;
using System.Text.Json;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Application.Extensions;

public static class SimpleNotificationServiceExtensions
{
    public static async Task<T> ReadBodyFromSNS<T>(this HttpRequest request)
    {
        string body;

        using (StreamReader reader = new (request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync();
        }

        var snsMessage = JsonSerializer.Deserialize<SnsWrapper>(body)!;

        var response = JsonSerializer.Deserialize<T>(snsMessage.Message)!;

        return response;
    }
}

record SnsWrapper(string Message);