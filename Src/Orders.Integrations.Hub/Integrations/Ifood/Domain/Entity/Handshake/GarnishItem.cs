using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record GarnishItem(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("parentUniqueId")] string ParentUniqueId,
    [property: JsonPropertyName("externalCode")] string ExternalCode,
    [property: JsonPropertyName("quantity")] int Quantity,
    [property: JsonPropertyName("index")] int Index,
    [property: JsonPropertyName("amount")] Amount Amount,
    [property: JsonPropertyName("reason")] string? Reason
);