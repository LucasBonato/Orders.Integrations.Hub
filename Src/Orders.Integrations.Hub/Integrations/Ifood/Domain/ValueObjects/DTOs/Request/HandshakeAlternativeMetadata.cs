using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record HandshakeAlternativeMetadata(
    [property: JsonPropertyName("amount")] Amount? Amount,
    [property: JsonPropertyName("additionalTimeInMinutes")] int? AdditionalTimeInMinutes,
    [property: JsonPropertyName("additionalTimeReason")] string? AdditionalTimeReason
);