﻿using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiOrderRejectRequest(
    [property: JsonPropertyName("reason")] string Reason,
    [property: JsonPropertyName("items_ids")] [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] List<string>? ItemIds,
    [property: JsonPropertyName("items_skus")] [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] List<string>? ItemSkus,
    [property: JsonPropertyName("cancel_type")] RappiOrderCancelType CancelType
);