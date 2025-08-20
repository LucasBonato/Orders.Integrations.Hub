using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record IfoodDisputeAlternative(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] [property: JsonConverter(typeof(JsonStringEnumConverter))] HandshakeAlternativeType Type,
    [property: JsonPropertyName("metadata")] IfoodDisputeAlternativeMetadata? Metadata,
    [property: JsonPropertyName("maxAmount")] Amount? Amount,
    [property: JsonPropertyName("allowedsAdditionalTimeInMinutes")] List<int>? AllowedsAdditionalTimeInMinutes,
    [property: JsonPropertyName("allowedsAdditionalTimeReasons")] List<string>? AllowedsAdditionalTimeReasons
);