using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity.Discount;

public record OrderDiscountSponsorshipValue(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("amount")] Price Amount
);