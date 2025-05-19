using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodOrderCancellationRequest(
    [property: JsonPropertyName("reason")] string Reason,
    [property: JsonPropertyName("cancellationCode")] string CancellationCode
) {
    public IfoodOrderCancellationRequest(string reason) : this(
        From(reason).ToString(),
        ((int)From(reason)).ToString()
    ) { }

    private static IfoodCancellationReasons From(string? reason)
    {
        int reasonCode = Convert.ToInt32(reason);
        if (reasonCode == 2) {
            return IfoodCancellationReasons.ITEM_UNAVAILABLE;
        }
        return (IfoodCancellationReasons)reasonCode;
    }
};