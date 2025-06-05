using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiAuthTokenRequest(
    [property: JsonPropertyName("client_id")] string ClienteId,
    [property: JsonPropertyName("client_secret")] string ClienteSecret,
    [property: JsonPropertyName("audience")] string Audience,
    [property: JsonPropertyName("grant_type")] string GrantType = "client_credentials"
);