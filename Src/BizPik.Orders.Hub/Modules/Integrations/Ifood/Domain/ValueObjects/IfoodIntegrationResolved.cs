namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects;

public record IfoodIntegrationResolved(
    string IfoodMerchantId,
    bool AutoAccept
);