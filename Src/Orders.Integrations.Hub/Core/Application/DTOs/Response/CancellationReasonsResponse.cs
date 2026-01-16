namespace Orders.Integrations.Hub.Core.Application.DTOs.Response;

public record CancellationReasonsResponse(
    int Code,
    string? Name,
    string? Description
);