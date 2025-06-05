using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs.Request;

public record OrderCancellationReasonRequest(
    [property: JsonPropertyName("orderId")] string OrderId,
    [property: JsonPropertyName("integration")] OrderIntegration Integration
);