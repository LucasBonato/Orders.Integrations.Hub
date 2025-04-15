using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Common.Orders.Domain.Entity.Discount;

public record OrderDiscountSponsorshipValue(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("amount")] Price Amount
);