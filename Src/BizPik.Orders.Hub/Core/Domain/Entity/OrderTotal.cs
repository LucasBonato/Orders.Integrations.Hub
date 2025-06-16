using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.Entity;

public record OrderTotal(
    [property: JsonPropertyName("itemsPrice")] Price ItemsPrice,
    [property: JsonPropertyName("otherFees")] Price OtherFees,
    [property: JsonPropertyName("discount")] Price? Discount,
    [property: JsonPropertyName("orderAmount")] Price OrderAmount
);