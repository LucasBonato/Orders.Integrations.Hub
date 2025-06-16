using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.Response;

public record CancellationReasonsResponse(
    [property: JsonPropertyName("code")] int Code,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("description")] string? Description
);