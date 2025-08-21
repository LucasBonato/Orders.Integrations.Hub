using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Item;

public record OrderItemOption(
    string Id,
    int Index,
    string Name,
    string ExternalCode,
    string ImageUrl,
    OrderUnit Unit,
    string Ean,
    int Quantity,
    string SpecialInstructions,
    Price UnitPrice,
    Price TotalPrice
);