using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record IfoodDisputeAlternativeMetadata(
    [property: JsonPropertyName("maxAmount")] Amount? Amount,
    [property: JsonPropertyName("allowedsAdditionalTimeInMinutes")] List<int>? AllowedsAdditionalTimeInMinutes,
    [property: JsonPropertyName("allowedsAdditionalTimeReasons")] List<string>? AllowedsAdditionalTimeReasons
);