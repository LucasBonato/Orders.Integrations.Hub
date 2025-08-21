using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

public record RappiAvailabilityItemStatusResponse(
    long ItemId,
    string ItemType, // this is a enum
    RappiAvailabilityState StockOutState
);