namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderCustomer(
    string Id,
    string Name,
    string DocumentNumber,
    string? Email,
    Phone Phone,
    int OrdersCountOnMerchant
);