namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record Price(
    decimal Value,
    string Currency
);