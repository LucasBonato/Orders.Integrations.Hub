namespace Orders.Integrations.Hub.Core.Domain.Entity.Dispute;

public record OrderDispute(
    string DisputeId,
    string Message,
    string Action,
    string TimeoutAction,
    DateTime CreatedAt,
    DateTime ExpiresAt,
    List<DisputeAlternative>? Alternatives,
    List<DisputeEvidence>? Evidences,
    List<DisputeItem>? Items,
    List<DisputeItemOption>? Options,
    List<string>? CancellationReasons
);