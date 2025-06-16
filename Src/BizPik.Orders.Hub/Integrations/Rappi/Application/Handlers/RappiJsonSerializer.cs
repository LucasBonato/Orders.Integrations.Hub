using System.Text.Json;
using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Domain.Contracts;

namespace BizPik.Orders.Hub.Integrations.Rappi.Application.Handlers;

public class RappiJsonSerializer : ICustomJsonSerializer
{
    public static readonly JsonSerializerOptions Options = new() {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper) }
    };

    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    public T? Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, Options);
}