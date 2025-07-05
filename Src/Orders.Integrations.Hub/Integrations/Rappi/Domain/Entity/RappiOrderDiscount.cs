using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderDiscount(
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("product_id")] long? ProductId,
    [property: JsonPropertyName("sku")] string? Sku,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("raw_value")] decimal RawValue,
    [property: JsonPropertyName("value_type")] [property: JsonConverter(typeof(JsonStringEnumConverter))] RappiOrderDiscountValueType ValueType,
    [property: JsonPropertyName("max_value")] decimal? MaxValue,
    [property: JsonPropertyName("includes_toppings")] bool IncludesToppings,
    [property: JsonPropertyName("percentage_by_rappi")] decimal PercentageByRappi,
    [property: JsonPropertyName("percentage_by_partners")] decimal PercentageByPartners,
    [property: JsonPropertyName("amount_by_rappi")] decimal AmountByRappi,
    [property: JsonPropertyName("amount_by_partner")] decimal AmountByPartner,
    [property: JsonPropertyName("discount_product_units")] decimal DiscountProductUnits,
    [property: JsonPropertyName("discount_product_unit_value")] decimal? DiscountProductUnitValue
);