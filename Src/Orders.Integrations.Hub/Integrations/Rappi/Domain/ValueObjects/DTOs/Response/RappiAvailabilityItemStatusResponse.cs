using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

public record RappiAvailabilityItemStatusResponse(
    [property: JsonPropertyName("item_id")] long ItemId,
    [property: JsonPropertyName("item_type")] string ItemType, // this is a enum
    [property: JsonPropertyName("stock_out_state")] RappiAvailabilityState StockOutState
);