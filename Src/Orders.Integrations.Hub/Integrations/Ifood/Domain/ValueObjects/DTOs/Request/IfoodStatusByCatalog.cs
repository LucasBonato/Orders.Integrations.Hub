namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodStatusByCatalog(
    string Status,
    string CatalogContext
);