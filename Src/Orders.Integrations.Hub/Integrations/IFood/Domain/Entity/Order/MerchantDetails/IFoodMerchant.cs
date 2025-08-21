namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.MerchantDetails;

public record IFoodMerchant(
    string Id,
    string Name,
    string CorporateName,
    string Description,
    decimal AverageTicket,
    bool Exclusive,
    string Type, // enum
    string Status, // enum
    DateTime CreatedAt,
    MerchantAddress Address,
    MerchantOperations Operations
);