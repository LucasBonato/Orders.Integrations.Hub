namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;

public record IFoodCancellationReasonResponse(
    string CancelCodeId,
    string Description
);