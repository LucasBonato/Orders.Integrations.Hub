using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public class DisputeAlternativeMetadata(
    Amount? Amount,
    List<int>? AllowedsAdditionalTimeInMinutes,
    List<NegotiationReasons>? AllowedsAdditionalTimeReasons
)
{
    [JsonPropertyName("maxAmount")]
    public Amount? Amount { get; init; } = Amount;

    [JsonPropertyName("allowedsAdditionalTimeInMinutes")]
    public List<int>? AllowedsAdditionalTimeInMinutes { get; init; } = AllowedsAdditionalTimeInMinutes;

    [JsonPropertyName("allowedsAdditionalTimeReasons")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public List<NegotiationReasons>? AllowedsAdditionalTimeReasons { get; init; } = AllowedsAdditionalTimeReasons;
}