namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Item;

public record Scale(
    decimal Price,
    int MinQuantity
);