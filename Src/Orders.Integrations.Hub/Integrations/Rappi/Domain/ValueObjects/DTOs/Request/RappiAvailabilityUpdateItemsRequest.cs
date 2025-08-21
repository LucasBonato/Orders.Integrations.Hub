namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiAvailabilityUpdateItemsRequest(
    string StoreIntegrationId,
    RappiAvailabilityItem Items
) {
    public static RappiAvailabilityUpdateItemsRequest NewTurnOff(string storeIntegrationId, List<string> skus)
        => new(storeIntegrationId, new RappiAvailabilityItem(TurnOn: [], TurnOff: skus));

    public static RappiAvailabilityUpdateItemsRequest NewTurnOn(string storeIntegrationId, List<string> skus)
        => new(storeIntegrationId, new RappiAvailabilityItem(TurnOn: skus, TurnOff: []));
};