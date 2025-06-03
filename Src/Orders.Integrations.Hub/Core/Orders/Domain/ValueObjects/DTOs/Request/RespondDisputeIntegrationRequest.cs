using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.Request;

public record RespondDisputeIntegrationRequest(
    [property: JsonPropertyName("disputeId")] string DisputeId,
    [property: JsonPropertyName("integration")] OrderIntegration Integration,
    [property: JsonPropertyName("type")] DisputeResponseType Type,
    [property: JsonPropertyName("alternativeId")] string? AlternativeId,
    [property: JsonPropertyName("disputeResponse")] RespondDisputeResponse DisputeResponse
);