using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

public record IFoodWebhookRequest(
    string Id,
    IFoodOrderStatus Code,
    IFoodFullOrderStatus FullCode,
    string OrderId,
    string MerchantId,
    List<string>? MerchantIds,
    DateTime CreatedAt,
    string? SalesChannel,
    Dictionary<string, object>? Metadata
);