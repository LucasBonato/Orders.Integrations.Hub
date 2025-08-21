namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

public record IFoodPatchProductStatusRequest(
    string ItemId,
    string Status,
    IFoodStatusByCatalog[] StatusByCatalog
){
    public static IFoodPatchProductStatusRequest Enable(string itemId, IFoodStatusByCatalog[] statusByCatalog)
        => new(
            ItemId: itemId,
            Status: "AVAILABLE",
            StatusByCatalog: statusByCatalog
        );

    public static IFoodPatchProductStatusRequest Disable(string itemId, IFoodStatusByCatalog[] statusByCatalog)
        => new(
            ItemId: itemId,
            Status: "UNAVAILABLE",
            StatusByCatalog: statusByCatalog
        );
};