namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

public record IfoodError(
    string Code,
    string Message,
    string Field,
    List<string?> Details
);