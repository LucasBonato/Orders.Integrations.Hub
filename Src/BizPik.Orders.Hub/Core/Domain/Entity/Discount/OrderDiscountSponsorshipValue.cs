using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Entity.Discount;

public record OrderDiscountSponsorshipValue(
    [property: JsonPropertyName("name")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderSponsorshipName Name,
    [property: JsonPropertyName("amount")] Price Amount,
    [property: JsonPropertyName("description")] string? Description
);