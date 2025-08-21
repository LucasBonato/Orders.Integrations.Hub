namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderTotalsOtherTotals(
    decimal? TotalRappiCredits,
    decimal? TotalRappiPay,
    decimal? Tip
);