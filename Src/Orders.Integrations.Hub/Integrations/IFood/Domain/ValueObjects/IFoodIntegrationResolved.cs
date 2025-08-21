namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects;

public record IFoodIntegrationResolved(
    string IfoodMerchantId,
    bool AutoAccept
);