using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Request;

public record Food99StatusChangeRequest(
    string OrderId,
    string? AuthToken,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] int? ReasonId,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Reason
);