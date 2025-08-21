using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodWebhookRequest(
    string Id,
    IfoodOrderStatus Code,
    IfoodFullOrderStatus FullCode,
    string OrderId,
    string MerchantId,
    List<string>? MerchantIds,
    DateTime CreatedAt,
    string? SalesChannel,
    Dictionary<string, object>? Metadata
);