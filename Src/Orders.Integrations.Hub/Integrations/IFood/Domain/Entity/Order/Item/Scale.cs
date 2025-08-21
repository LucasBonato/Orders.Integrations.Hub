namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Item;

public record Scale(
    decimal Price,
    int MinQuantity
);