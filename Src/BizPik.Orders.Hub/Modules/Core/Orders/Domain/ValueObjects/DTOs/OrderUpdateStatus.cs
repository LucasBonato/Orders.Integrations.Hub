using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Core.Orders.Domain.ValueObjects.DTOs;

public record OrderUpdateStatus(
    [property: JsonPropertyName("orderId")] string OrderId,
    [property: JsonPropertyName("sourceAppId")] string SourceAppId,
    [property: JsonPropertyName("type")] OrderEventType Type,
    [property: JsonPropertyName("createAt")] DateTime CreateAt,
    [property: JsonPropertyName("fromIntegration")] bool FromIntegration
);