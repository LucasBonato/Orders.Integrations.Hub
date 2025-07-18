﻿using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

public record RappiWebhookEventsResponse(
    [property: JsonPropertyName("event")] string Event,
    [property: JsonPropertyName("stores")] List<RappiWebhookStore> Stores
);