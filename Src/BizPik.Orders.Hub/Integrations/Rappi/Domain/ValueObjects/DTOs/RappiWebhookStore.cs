using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiWebhookStore(
    [property: JsonPropertyName("store_id")] string StoreId,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("state")] RappiAvailabilityState State
);