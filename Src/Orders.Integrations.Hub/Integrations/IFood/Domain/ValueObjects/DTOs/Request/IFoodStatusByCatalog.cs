namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

public record IFoodStatusByCatalog(
    string Status,
    string CatalogContext
);