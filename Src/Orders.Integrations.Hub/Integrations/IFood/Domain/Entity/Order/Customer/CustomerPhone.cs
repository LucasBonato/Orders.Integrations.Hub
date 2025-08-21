namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Customer;

public record CustomerPhone(
    string Number,
    string Localizer,
    DateTime LocalizerExpiration
);