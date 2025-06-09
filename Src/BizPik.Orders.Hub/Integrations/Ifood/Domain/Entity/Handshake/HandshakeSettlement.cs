using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record HandshakeSettlement(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("disputeId")] string? DisputeId,
    [property: JsonPropertyName("status")] [property: JsonConverter(typeof(JsonStringEnumConverter))] Status Status,
    [property: JsonPropertyName("reason")] string? Reason,
    [property: JsonPropertyName("selectedDisputeAlternative")] SelectedDisputeAlternative SelectedDisputeAlternative
);