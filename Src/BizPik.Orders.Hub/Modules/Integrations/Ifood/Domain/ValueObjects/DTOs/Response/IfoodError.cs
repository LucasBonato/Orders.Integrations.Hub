using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

public record IfoodError(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("field")] string Field,
    [property: JsonPropertyName("details")] List<string?> Details
);