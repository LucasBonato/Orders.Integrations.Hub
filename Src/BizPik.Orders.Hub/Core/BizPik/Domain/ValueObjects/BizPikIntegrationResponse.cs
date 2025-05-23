using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.BizPik.Domain.ValueObjects;

public record BizPikIntegrationResponse(
    [property: JsonPropertyName("companyId")] int? CompanyId,
    [property: JsonPropertyName("integrationId")] int? IntegrationId,
    [property: JsonPropertyName("integratorId")] int? IntegratorId,
    [property: JsonPropertyName("creatorId")] int? CreatorId,
    [property: JsonPropertyName("integrationType")] int? IntegrationType,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("isActive")] bool? IsActive,
    [property: JsonPropertyName("settings")] IReadOnlyList<BizPikIntegrationSetting> Settings,
    [property: JsonPropertyName("extraSettings")] IReadOnlyList<object> ExtraSettings,
    [property: JsonPropertyName("lambdaUrl")] object LambdaUrl,
    [property: JsonPropertyName("apiKey")] string ApiKey,
    [property: JsonPropertyName("userId")] string UserId
);