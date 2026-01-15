using System.Text.Json;
using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Application.Integration;

namespace Orders.Integrations.Hub.Core.Infrastructure;

public sealed class IntegrationKeyJsonConverter : JsonConverter<IntegrationKey> {
    public override IntegrationKey? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) {
        string value = reader.GetString()?? throw new JsonException("Integration key is required");

        return IntegrationKey.From(value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        IntegrationKey value,
        JsonSerializerOptions options
    ) => writer.WriteStringValue(value.Value);
}