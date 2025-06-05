using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodPatchProductStatusRequest(
    [property: JsonPropertyName("itemId")] string ItemId,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("statusByCatalog")] IfoodStatusByCatalog[] StatusByCatalog
){
    public static IfoodPatchProductStatusRequest Enable(string itemId, IfoodStatusByCatalog[] statusByCatalog)
        => new(
            ItemId: itemId,
            Status: "AVAILABLE",
            StatusByCatalog: statusByCatalog
        );

    public static IfoodPatchProductStatusRequest Disable(string itemId, IfoodStatusByCatalog[] statusByCatalog)
        => new(
            ItemId: itemId,
            Status: "UNAVAILABLE",
            StatusByCatalog: statusByCatalog
        );
};