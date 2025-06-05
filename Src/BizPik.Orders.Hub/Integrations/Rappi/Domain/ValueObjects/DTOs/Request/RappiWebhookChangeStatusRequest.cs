using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;

public record RappiWebhookChangeStatusRequest(
    [property: JsonPropertyName("stores")] RappiWebhookStatusStore Stores
);