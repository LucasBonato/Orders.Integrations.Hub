using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodWebhookRequest(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("code")] [property: JsonConverter(typeof(JsonStringEnumConverter))] IfoodOrderStatus Code,
    [property: JsonPropertyName("fullCode")] [property: JsonConverter(typeof(JsonStringEnumConverter))] IfoodFullOrderStatus FullCode,
    [property: JsonPropertyName("orderId")] string OrderId,
    [property: JsonPropertyName("merchantId")] string MerchantId,
    [property: JsonPropertyName("merchantIds")] List<string>? MerchantIds,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("salesChannel")] string? SalesChannel,
    [property: JsonPropertyName("metadata")] Dictionary<string, object>? Metadata
);