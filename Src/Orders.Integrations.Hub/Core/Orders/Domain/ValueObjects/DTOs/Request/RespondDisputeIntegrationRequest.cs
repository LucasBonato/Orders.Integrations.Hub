using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.Request;

public record RespondDisputeIntegrationRequest(
    [property: JsonPropertyName("disputeId")] string DisputeId,
    [property: JsonPropertyName("alternativeId")] string? AlternativeId,
    [property: JsonPropertyName("type")] DisputeResponseType Type,
    [property: JsonPropertyName("metadata")] Dictionary<string, object> Metadata
);