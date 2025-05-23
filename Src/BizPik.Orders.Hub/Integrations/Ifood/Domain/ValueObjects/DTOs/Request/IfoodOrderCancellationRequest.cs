using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodOrderCancellationRequest(
    [property: JsonPropertyName("reason")] string Reason,
    [property: JsonPropertyName("cancellationCode")] string CancellationCode
) {
    public IfoodOrderCancellationRequest(string reason) : this(
        FromBizPik(reason).ToString(),
        ((int)FromBizPik(reason)).ToString()
    ) { }

    private static IfoodCancellationReasons FromBizPik(string? reason)
    {
        int reasonCode = Convert.ToInt32(reason);
        if (reasonCode == 2) {
            return IfoodCancellationReasons.ITEM_UNAVAILABLE;
        }
        return (IfoodCancellationReasons)reasonCode;
    }
};