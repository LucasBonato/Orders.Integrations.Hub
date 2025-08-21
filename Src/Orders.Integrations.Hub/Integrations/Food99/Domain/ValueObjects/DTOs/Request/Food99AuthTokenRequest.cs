using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Request;

public record Food99AuthTokenRequest(
    [property: JsonPropertyName("app_id")] string AppId,
    [property: JsonPropertyName("app_secret")] string AppSecret,
    [property: JsonPropertyName("app_shop_id")] string AppShopId    
);