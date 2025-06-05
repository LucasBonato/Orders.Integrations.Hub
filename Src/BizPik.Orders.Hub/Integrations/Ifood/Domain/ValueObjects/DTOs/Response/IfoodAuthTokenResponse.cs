using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

public record IfoodAuthTokenResponse(
    [property: JsonPropertyName("accessToken")] string AccessToken,
    [property: JsonPropertyName("type")] string Tyoe,
    [property: JsonPropertyName("expiresIn")] int ExpiresIn
);