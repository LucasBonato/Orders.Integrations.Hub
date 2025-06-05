using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

public record RappiWebhookRemoveStoresResponse(
    [property: JsonPropertyName("stores")] List<string> Stores,
    [property: JsonPropertyName("message")] string Message
);