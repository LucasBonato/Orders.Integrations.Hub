using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record DisputeAlternativeMetadata(
    [property: JsonPropertyName("maxAmount")] Amount? Amount,
    [property: JsonPropertyName("allowedsAdditionalTimeInMinutes")] List<int>? AllowedsAdditionalTimeInMinutes,
    [property: JsonPropertyName("allowedsAdditionalTimeReasons")] List<string>? AllowedsAdditionalTimeReasons
);