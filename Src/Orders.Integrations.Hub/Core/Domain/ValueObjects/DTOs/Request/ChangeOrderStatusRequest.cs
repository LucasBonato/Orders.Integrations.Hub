using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;

public record ChangeOrderStatusRequest(
    [property: JsonPropertyName("orderId")] string OrderId,
    [property: JsonPropertyName("externalId")] string ExternalId,
    [property: JsonPropertyName("merchantId")] string MerchantId,
    [property: JsonPropertyName("status")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderEventType Status,
    [property: JsonPropertyName("integration")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderIntegration Integration,
    [property: JsonPropertyName("cancellationReason")] string? CancellationReason,
    [property: JsonPropertyName("cancellationMetadata")] CancellationMetadata? CancellationMetadata
);