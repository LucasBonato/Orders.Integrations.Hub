using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Item;

public record OrderItem(
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
    Price TotalPrice,
    Price OptionsPrice,
    IReadOnlyList<OrderItemOption> Options
);