using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.Entity.Dispute;

public record DisputeItem(
    [property: JsonPropertyName("externalId")] string ExternalId,
    [property: JsonPropertyName("externalUniqueId")] string? ExternalUniqueId,
    [property: JsonPropertyName("sku")] string Sku,
    [property: JsonPropertyName("index")] int Index,
    [property: JsonPropertyName("quantity")] int Quantity,
    [property: JsonPropertyName("price")] Price Price,
    [property: JsonPropertyName("reasonMessage")] string ReasonMessage
);