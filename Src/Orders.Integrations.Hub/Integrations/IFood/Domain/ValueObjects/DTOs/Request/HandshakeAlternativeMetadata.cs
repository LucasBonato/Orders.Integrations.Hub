using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

public record HandshakeAlternativeMetadata(
    Amount? Amount,
    int? AdditionalTimeInMinutes,
    string? AdditionalTimeReason
);