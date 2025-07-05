using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiAvailabilityItemsStatusRequest(
    [property: JsonPropertyName("store_id")] string StoreId,
    [property: JsonPropertyName("item_ids")] List<string> ItemIds
);