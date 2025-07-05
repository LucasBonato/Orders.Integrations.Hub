using System.Text.Json;
using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Domain.Contracts;

namespace Orders.Integrations.Hub.Core.Application.Middlewares;

public class CoreJsonSerializer : ICustomJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper) }
    };

    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    public T? Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, Options);
}