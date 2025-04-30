using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookRemoveStoresRequest(
    [property: JsonPropertyName("stores")] List<string> Stores
);