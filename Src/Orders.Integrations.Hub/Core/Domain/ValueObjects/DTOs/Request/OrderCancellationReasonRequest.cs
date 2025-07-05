using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Request;

public record OrderCancellationReasonRequest(
    [property: JsonPropertyName("orderId")] string OrderId,
    [property: JsonPropertyName("integration")] OrderIntegration Integration
);