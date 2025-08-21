using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record IfoodDisputeAlternative(
    string Id,
    HandshakeAlternativeType Type,
    IfoodDisputeAlternativeMetadata? Metadata,
    Amount? MaxAmount,
    List<int>? AllowedsAdditionalTimeInMinutes,
    List<string>? AllowedsAdditionalTimeReasons
);