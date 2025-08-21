using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Item;

public record Option(
    int Index,
    string Id,
    string Name,
    string GroupName,
    string Type,
    string ExternalCode,
    int Quantity,
    Unit Unit,
    string Ean,
    decimal UnitPrice,
    decimal Addition,
    decimal Price,
    IReadOnlyList<Customization> Customizations
);