using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Item;

public record Item(
    int Index,
    string Id,
    string UniqueId,
    string ImageUrl,
    string? ExternalCode,
    string Ean,
    string Name,
    string Type,
    int Quantity,
    Unit Unit,
    decimal UnitPrice,
    decimal Price,
    ScalePrices ScalePrices,
    decimal OptionsPrice,
    decimal TotalPrice,
    string Observations,
    IReadOnlyList<Option>? Options
);