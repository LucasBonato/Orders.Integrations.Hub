namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Item;

public record Customization(
    string Id,
    string Name,
    string GroupName,
    string ExternalCode,
    string Type,
    int Quantity,
    decimal UnitPrice,
    decimal Addition,
    decimal Price
);