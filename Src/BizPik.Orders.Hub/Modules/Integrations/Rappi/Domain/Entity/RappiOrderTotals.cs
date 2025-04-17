using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.Entity;

public record RappiOrderTotals(
    [property: JsonPropertyName("total_products")] decimal? TotalProducts ,
    [property: JsonPropertyName("total_discounts")] decimal? TotalDiscounts ,
    [property: JsonPropertyName("total_products_with_discount")] double? TotalProductsWithDiscount ,
    [property: JsonPropertyName("total_products_without_discount")] double? TotalProductsWithoutDiscount ,
    [property: JsonPropertyName("total_other_discounts")] double? TotalOtherDiscounts ,
    [property: JsonPropertyName("total_order")] double? TotalOrder ,
    [property: JsonPropertyName("total_to_pay")] double? TotalToPay ,
    [property: JsonPropertyName("charges")] RappiOrderTotalsCharges Charges,
    [property: JsonPropertyName("other_totals")] RappiOrderTotalsOtherTotals OtherTotals
);