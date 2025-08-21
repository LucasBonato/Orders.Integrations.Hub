using Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record HandshakeAlternativeMetadata(
    Amount? Amount,
    int? AdditionalTimeInMinutes,
    string? AdditionalTimeReason
);