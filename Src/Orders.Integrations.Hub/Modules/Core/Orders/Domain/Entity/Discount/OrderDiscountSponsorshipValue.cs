using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Domain.Entity.Discount;

public record OrderDiscountSponsorshipValue(
    [property: JsonPropertyName("name")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderSponsorshipName Name,
    [property: JsonPropertyName("amount")] Price Amount,
    [property: JsonPropertyName("description")] string? Description
);