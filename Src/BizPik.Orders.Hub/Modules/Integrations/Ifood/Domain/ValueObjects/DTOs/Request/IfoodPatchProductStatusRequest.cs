using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodPatchProductStatusRequest(
    [property: JsonPropertyName("itemId")] string ItemId,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("statusByCatalog")] IfoodStatusByCatalog[] StatusByCatalog
);