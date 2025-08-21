namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

public record IfoodCancellationReasonResponse(
    string CancelCodeId,
    string Description
);