using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;

public record SelectedDisputeAlternative(
    string Id,
    HandshakeAlternativeType Type,
    IFoodDisputeAlternativeMetadata Metadata
);