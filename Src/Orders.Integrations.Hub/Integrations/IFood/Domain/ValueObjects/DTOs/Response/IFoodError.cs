namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;

public record IFoodError(
    string Code,
    string Message,
    string Field,
    List<string?> Details
);