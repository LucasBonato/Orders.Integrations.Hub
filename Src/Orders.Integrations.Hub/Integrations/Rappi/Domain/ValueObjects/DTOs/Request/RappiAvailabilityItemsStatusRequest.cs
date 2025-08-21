namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiAvailabilityItemsStatusRequest(
    string StoreId,
    List<string> ItemIds
);