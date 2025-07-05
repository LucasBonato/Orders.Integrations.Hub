using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record HandshakeSettlement(
    [property: JsonPropertyName("disputeId")] string DisputeId,
    [property: JsonPropertyName("status")] [property: JsonConverter(typeof(JsonStringEnumConverter))] Status Status,
    [property: JsonPropertyName("reason")] string? Reason,
    [property: JsonPropertyName("selectedDisputeAlternative")] SelectedDisputeAlternative SelectedDisputeAlternative,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt
);