using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

public record RappiAvailabilityItemStatusResponse(
    [property: JsonPropertyName("item_id")] long ItemId,
    [property: JsonPropertyName("item_type")] string ItemType, // this is a enum
    [property: JsonPropertyName("stock_out_state")] RappiAvailabilityState StockOutState
);