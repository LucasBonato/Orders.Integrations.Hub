using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Adapter.Dtos.Entity.Discount;

public record OrderDiscount(
    [property: JsonPropertyName("amount")] Price Amount,
    [property: JsonPropertyName("target")] string Target,
    [property: JsonPropertyName("targetId")] string TargetId,
    [property: JsonPropertyName("sponsorshipValues")] IReadOnlyList<OrderDiscountSponsorshipValue> SponsorshipValues
);