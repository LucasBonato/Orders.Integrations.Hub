using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderItem(
    decimal? Price,
    string? Sku,
    string Id,
    string Name,
    RappiOrderItemType Type,
    string? Comments,
    double? UnitPriceWithDiscount,
    double? UnitPriceWithoutDiscount,
    double? PercentageDiscount,
    int Quantity,
    List<RappiOrderSubItem> Subitems
);