using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderSubItem(
    string? Sku,
    string Id,
    string Name,
    RappiOrderItemType Type,
    decimal? Price,
    double? PercentageDiscount,
    int? Quantity
);