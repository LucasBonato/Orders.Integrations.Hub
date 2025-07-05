using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Orders.Integrations.Hub.Core.Application.Extensions;

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