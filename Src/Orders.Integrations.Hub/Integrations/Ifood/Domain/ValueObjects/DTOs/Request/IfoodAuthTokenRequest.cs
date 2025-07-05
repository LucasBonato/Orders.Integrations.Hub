using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodAuthTokenRequest(
    [property: JsonPropertyName("grantType")] string GrantType,
    [property: JsonPropertyName("clientId")] string ClientId,
    [property: JsonPropertyName("clientSecret")] string ClientSecret
);