using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderDiscount(
    decimal Value,
    string Description,
    string Title,
    long? ProductId,
    string? Sku,
    string Type,
    decimal RawValue,
    RappiOrderDiscountValueType ValueType,
    decimal? MaxValue,
    bool IncludesToppings,
    decimal PercentageByRappi,
    decimal PercentageByPartners,
    decimal AmountByRappi,
    decimal AmountByPartner,
    decimal DiscountProductUnits,
    decimal? DiscountProductUnitValue
);