namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Customer;

public record CustomerPhone(
    string Number,
    string Localizer,
    DateTime LocalizerExpiration
);