using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Orders.Domain.Entity;

namespace BizPik.Orders.Hub.Core.Orders.Domain.ValueObjects.DTOs.Request;

// The name ends with response because it's the response to the dispute
public record RespondDisputeResponse(
    [property: JsonPropertyName("reason")] [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Reason,
    [property: JsonPropertyName("detailsReason")] [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? DetailsReason,
    [property: JsonPropertyName("type")] [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Type,
    [property: JsonPropertyName("price")] [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] Price? Price,
    [property: JsonPropertyName("additionalTimeInMinutes")] [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] int? AdditionalTimeInMinutes,
    [property: JsonPropertyName("additionalTimeReason")] [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? AdditionalTimeReason
);