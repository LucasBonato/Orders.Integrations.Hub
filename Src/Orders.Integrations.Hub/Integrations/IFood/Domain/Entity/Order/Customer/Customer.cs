namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Customer;

public record Customer(
    string Id,
    string Name,
    string DocumentNumber,
    int OrdersCountOnMerchant,
    CustomerPhone? Phone,
    string Segmentation
);