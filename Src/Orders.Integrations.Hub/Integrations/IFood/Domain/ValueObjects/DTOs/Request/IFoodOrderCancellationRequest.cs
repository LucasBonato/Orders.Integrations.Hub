using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;

public record IFoodOrderCancellationRequest(
    string Reason,
    string CancellationCode
) {
    public IFoodOrderCancellationRequest(string reason) : this(
        FromInternal(reason).ToString(),
        ((int)FromInternal(reason)).ToString()
    ) { }

    private static IFoodCancellationReasons FromInternal(string? reason)
    {
        int reasonCode = Convert.ToInt32(reason);
        if (reasonCode == 2) {
            return IFoodCancellationReasons.ITEM_UNAVAILABLE;
        }
        return (IFoodCancellationReasons)reasonCode;
    }
};