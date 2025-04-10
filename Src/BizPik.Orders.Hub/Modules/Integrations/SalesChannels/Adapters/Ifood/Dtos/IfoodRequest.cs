using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Saleschannels.Adapters.Ifood.Dtos;

public record IfoodRequest(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("fullCode")] string FullCode,
    [property: JsonPropertyName("orderId")] string? OrderId,
    [property: JsonPropertyName("merchantId")] string? MerchantId,
    [property: JsonPropertyName("merchantIds")] List<string>? MerchantIds,
    [property: JsonPropertyName("createdAt")] DateTime? CreatedAt,
    [property: JsonPropertyName("metadata")] Dictionary<string, object>? Metadata
);