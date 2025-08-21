namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderTotals(
    decimal? TotalProducts ,
    decimal? TotalDiscounts ,
    decimal? TotalProductsWithDiscount ,
    decimal? TotalProductsWithoutDiscount ,
    decimal? TotalOtherDiscounts ,
    decimal? TotalOrder ,
    decimal? TotalToPay ,
    RappiOrderTotalsCharges Charges,
    RappiOrderTotalsOtherTotals OtherTotals
);