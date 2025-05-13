using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodOrderCancellationRequest(
    [property: JsonPropertyName("reason")] string Reason,
    [property: JsonPropertyName("cancellationCode")] string CancellationCode
) {
    public IfoodOrderCancellationRequest(string reason) : this(From(reason).ToString(), ((int)From(reason)).ToString()) { }

    public static IfoodCancellationReasons From(string? reason)
        => reason switch
        {
            "OutOfStock" => IfoodCancellationReasons.ITEM_UNAVAILABLE,
            _ => IfoodCancellationReasons.MERCHANT_SYSTEM_PROBLEMS
        };
};