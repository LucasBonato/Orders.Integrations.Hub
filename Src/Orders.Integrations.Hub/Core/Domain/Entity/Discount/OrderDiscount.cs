using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Orders.Domain.Entity.Discount;

public record OrderDiscount(
    [property: JsonPropertyName("amount")] Price Amount,
    [property: JsonPropertyName("target")] [property: JsonConverter(typeof(JsonStringEnumConverter))] DiscountTarget Target,
    [property: JsonPropertyName("targetId")] string TargetId,
    [property: JsonPropertyName("sponsorshipValues")] IReadOnlyList<OrderDiscountSponsorshipValue> SponsorshipValues
);