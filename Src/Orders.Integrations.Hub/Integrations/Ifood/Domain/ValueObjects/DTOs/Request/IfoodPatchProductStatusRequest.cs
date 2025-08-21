namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodPatchProductStatusRequest(
    string ItemId,
    string Status,
    IfoodStatusByCatalog[] StatusByCatalog
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