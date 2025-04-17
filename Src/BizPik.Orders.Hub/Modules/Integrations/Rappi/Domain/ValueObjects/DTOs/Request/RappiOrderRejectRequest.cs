using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiOrderRejectRequest(
    [property: JsonPropertyName("reason")] string Reason,
    [property: JsonPropertyName("items_ids")] List<string> ItemIds,
    [property: JsonPropertyName("items_skus")] List<string> ItemSkus,
    [property: JsonPropertyName("cancel_type")] RappiOrderCancelType CancelType
);