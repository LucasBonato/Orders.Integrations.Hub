namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Item;

public record ScalePrices(
    decimal DefaultPrice,
    IReadOnlyList<Scale> Scales
);