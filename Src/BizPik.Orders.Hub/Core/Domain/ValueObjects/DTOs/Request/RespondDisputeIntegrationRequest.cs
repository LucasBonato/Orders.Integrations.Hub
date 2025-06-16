using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.Request;

public record RespondDisputeIntegrationRequest(
    [property: JsonPropertyName("disputeId")] string DisputeId,
    [property: JsonPropertyName("integration")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderIntegration Integration,
    [property: JsonPropertyName("type")] [property: JsonConverter(typeof(JsonStringEnumConverter))] DisputeResponseType Type,
    [property: JsonPropertyName("alternativeId")] string? AlternativeId,
    [property: JsonPropertyName("disputeResponse")] RespondDisputeResponse DisputeResponse
);