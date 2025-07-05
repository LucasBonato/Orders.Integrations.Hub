namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects;

public record IfoodIntegrationResolved(
    string IfoodMerchantId,
    bool AutoAccept
);