namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderTotalsCharges(
    decimal? Shipping,
    decimal? ServiceFee
);