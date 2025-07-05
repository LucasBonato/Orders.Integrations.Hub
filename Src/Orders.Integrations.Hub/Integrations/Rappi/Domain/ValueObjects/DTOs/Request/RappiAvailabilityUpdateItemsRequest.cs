using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiAvailabilityUpdateItemsRequest(
    [property: JsonPropertyName("store_integration_id")] string StoreIntegrationId,
    [property: JsonPropertyName("items")] RappiAvailabilityItem Items
) {
    public static RappiAvailabilityUpdateItemsRequest NewTurnOff(string storeIntegrationId, List<string> skus)
        => new(storeIntegrationId, new RappiAvailabilityItem(TurnOn: [], TurnOff: skus));

    public static RappiAvailabilityUpdateItemsRequest NewTurnOn(string storeIntegrationId, List<string> skus)
        => new(storeIntegrationId, new RappiAvailabilityItem(TurnOn: skus, TurnOff: []));
};