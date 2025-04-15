using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodStatusByCatalog(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("catalogContext")] string CatalogContext
);