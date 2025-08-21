using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Food99.Domain.Entity;
public record Food99ConfirmCancelOrder(    
    [property: JsonPropertyName("app_shop_id")] string AppShopId,
    [property: JsonPropertyName("order_id")] long OrderId,
    [property: JsonPropertyName("reason_id")] int? ReasonId = null,
    [property: JsonPropertyName("reason")] string? Reason = null
);

