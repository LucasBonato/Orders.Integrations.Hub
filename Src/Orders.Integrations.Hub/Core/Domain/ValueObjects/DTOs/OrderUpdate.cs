using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs;

public record OrderUpdate(
    [property: JsonPropertyName("orderId")] string OrderId,
    [property: JsonPropertyName("sourceAppId")] string SourceAppId,
    [property: JsonPropertyName("type")] OrderEventType Type,
    [property: JsonPropertyName("createAt")] DateTime CreateAt,
    [property: JsonPropertyName("dispute")] OrderDispute? Dispute,
    [property: JsonPropertyName("fromIntegration")] bool FromIntegration
);