using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodOrderCancellationRequest(
    [property: JsonPropertyName("reason")] string Reason,
    [property: JsonPropertyName("cancellationCode")] string CancellationCode
) {
    public IfoodOrderCancellationRequest(string reason) : this(FromBizPik(reason).ToString(), ((int)FromBizPik(reason)).ToString()) { }

    public static IfoodCancellationReasons FromBizPik(string? reason)
        => reason switch
        {
            "OutOfStock" => IfoodCancellationReasons.ITEM_UNAVAILABLE,
            _ => IfoodCancellationReasons.MERCHANT_SYSTEM_PROBLEMS
        };
};