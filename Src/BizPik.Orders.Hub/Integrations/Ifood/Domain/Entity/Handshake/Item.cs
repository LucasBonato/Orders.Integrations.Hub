using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record Item(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("uniqueId")] string UniqueId,
    [property: JsonPropertyName("externalCode")] string ExternalCode,
    [property: JsonPropertyName("quantity")] int Quantity,
    [property: JsonPropertyName("index")] int Index,
    [property: JsonPropertyName("amount")] Amount Amount,
    [property: JsonPropertyName("reason")] string? Reason
);