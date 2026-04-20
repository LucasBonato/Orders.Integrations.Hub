using System.Text.Json;
using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;

namespace Orders.Integrations.Hub.Integrations.Common.Serialization;

public class CommonJsonSerializer : ICustomJsonSerializer {
    private static readonly JsonSerializerOptions Options = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper) }
    };

    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    public T? Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, Options);
};