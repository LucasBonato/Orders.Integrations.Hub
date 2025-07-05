using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodStatusByCatalog(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("catalogContext")] string CatalogContext
);