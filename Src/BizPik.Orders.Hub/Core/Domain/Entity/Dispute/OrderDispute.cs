using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Entity.Dispute;

public record OrderDispute(
    [property: JsonPropertyName("disputeId")] string DisputeId,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("action")] string Action,
    [property: JsonPropertyName("timeoutAction")] string TimeoutAction,
    [property: JsonPropertyName("createdAt")] DateTime CreatedAt,
    [property: JsonPropertyName("expiresAt")] DateTime ExpiresAt,
    [property: JsonPropertyName("alternatives")] List<DisputeAlternative>? Alternatives,
    [property: JsonPropertyName("evidences")] List<DisputeEvidence>? Evidences,
    [property: JsonPropertyName("items")] List<DisputeItem>? Items,
    [property: JsonPropertyName("options")] List<DisputeItemOption>? Options,
    [property: JsonPropertyName("cancellationReasons")] List<string>? CancellationReasons
);