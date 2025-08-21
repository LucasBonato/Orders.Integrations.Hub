using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Dispute;

public record DisputeAlternative(
    string AlternativeId,
    HandshakeAlternativeType Type,
    Price? Price,
    List<int>? AllowedTimesInMinutes,
    List<string>? AllowedTimesReasons
);