namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Item;

public record ScalePrices(
    decimal DefaultPrice,
    IReadOnlyList<Scale> Scales
);