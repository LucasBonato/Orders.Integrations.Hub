namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.AdditionalFee;

public record AdditionalFee(
    string Type,
    string Description,
    string FullDescription,
    decimal Value,
    IReadOnlyList<Liability> Liabilities
);