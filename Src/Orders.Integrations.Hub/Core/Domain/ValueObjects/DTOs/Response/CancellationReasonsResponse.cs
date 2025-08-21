namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Response;

public record CancellationReasonsResponse(
    int Code,
    string? Name,
    string? Description
);