using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Common.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Common.Orders.Domain.ValueObjects.DTOs;

public record OrderUpdateStatus(
    [property: JsonPropertyName("orderId")] string OrderId,
    [property: JsonPropertyName("sourceAppId")] string SourceAppId,
    [property: JsonPropertyName("type")] OrderEventType Type,
    [property: JsonPropertyName("createAt")] DateTime CreateAt,
    [property: JsonPropertyName("fromIntegration")] bool FromIntegration
);