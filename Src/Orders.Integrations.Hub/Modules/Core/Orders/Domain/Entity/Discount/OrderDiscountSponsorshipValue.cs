using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity.Discount;

public record OrderDiscountSponsorshipValue(
    [property: JsonPropertyName("name")] OrderSponsorshipName Name,
    [property: JsonPropertyName("amount")] Price Amount
);