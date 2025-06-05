using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderTotals(
    [property: JsonPropertyName("total_products")] decimal? TotalProducts ,
    [property: JsonPropertyName("total_discounts")] decimal? TotalDiscounts ,
    [property: JsonPropertyName("total_products_with_discount")] decimal? TotalProductsWithDiscount ,
    [property: JsonPropertyName("total_products_without_discount")] decimal? TotalProductsWithoutDiscount ,
    [property: JsonPropertyName("total_other_discounts")] decimal? TotalOtherDiscounts ,
    [property: JsonPropertyName("total_order")] decimal? TotalOrder ,
    [property: JsonPropertyName("total_to_pay")] decimal? TotalToPay ,
    [property: JsonPropertyName("charges")] RappiOrderTotalsCharges Charges,
    [property: JsonPropertyName("other_totals")] RappiOrderTotalsOtherTotals OtherTotals
);