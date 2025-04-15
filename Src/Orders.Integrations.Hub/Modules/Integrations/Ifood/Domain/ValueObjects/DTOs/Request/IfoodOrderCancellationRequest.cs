using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;

public record IfoodOrderCancellationRequest(
    [property: JsonPropertyName("reason")] string Reason,
    [property: JsonPropertyName("cancellationCode")] string CancellationCode
) {
    public static IfoodCancellationReasons From(string reason)
        => reason switch
        {
            "OutOfStock" => IfoodCancellationReasons.ItemUnavailable,
            _ => IfoodCancellationReasons.SystemProblems
        };
};