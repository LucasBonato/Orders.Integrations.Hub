using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.Entity.Discount;

public record OrderDiscount(
    [property: JsonPropertyName("amount")] Price Amount,
    [property: JsonPropertyName("target")] string Target,
    [property: JsonPropertyName("targetId")] string TargetId,
    [property: JsonPropertyName("sponsorshipValues")] IReadOnlyList<OrderDiscountSponsorshipValue> SponsorshipValues
);