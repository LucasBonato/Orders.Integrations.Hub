using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;

public record IFoodDisputeAlternative(
    string Id,
    HandshakeAlternativeType Type,
    IFoodDisputeAlternativeMetadata? Metadata,
    Amount? MaxAmount,
    List<int>? AllowedsAdditionalTimeInMinutes,
    List<string>? AllowedsAdditionalTimeReasons
);