using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Response;

public record Food99RefreshTokenResponse(
    [property: JsonPropertyName("errno")] int Errno,
    [property: JsonPropertyName("errmsg")] string Errmsg,
    [property: JsonPropertyName("requestId")] string RequestId,
    [property: JsonPropertyName("time")] long Time,
    [property: JsonPropertyName("data")] bool Data
);