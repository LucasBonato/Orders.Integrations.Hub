using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

public record RappiWebhookPingResponse(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("description")] string Description
);