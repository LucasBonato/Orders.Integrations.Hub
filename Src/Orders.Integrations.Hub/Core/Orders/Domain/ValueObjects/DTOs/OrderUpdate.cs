using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Orders.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs;

public record OrderUpdateStatus(
    [property: JsonPropertyName("orderId")] string OrderId,
    [property: JsonPropertyName("sourceAppId")] string SourceAppId,
    [property: JsonPropertyName("type")] OrderEventType Type,
    [property: JsonPropertyName("createAt")] DateTime CreateAt,
    [property: JsonPropertyName("dispute")] OrderDispute? Dispute,
    [property: JsonPropertyName("fromIntegration")] bool FromIntegration
);