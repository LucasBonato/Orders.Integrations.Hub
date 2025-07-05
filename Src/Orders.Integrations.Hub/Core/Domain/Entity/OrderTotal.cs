using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderTotal(
    [property: JsonPropertyName("itemsPrice")] Price ItemsPrice,
    [property: JsonPropertyName("otherFees")] Price OtherFees,
    [property: JsonPropertyName("discount")] Price? Discount,
    [property: JsonPropertyName("orderAmount")] Price OrderAmount
);