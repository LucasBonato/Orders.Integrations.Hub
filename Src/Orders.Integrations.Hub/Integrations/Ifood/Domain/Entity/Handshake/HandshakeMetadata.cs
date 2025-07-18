﻿using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public class HandshakeMetadata(
    List<Item>? Items,
    List<GarnishItem>? GarnishItems,
    List<Media>? Evidences,
    List<string>? AcceptCancellationReasons
) {
    [JsonPropertyName("items")] public List<Item>? Items { get; init; } = Items;

    [JsonPropertyName("garnishItems")]
    public List<GarnishItem>? GarnishItems { get; init; } = GarnishItems;

    [JsonPropertyName("evidences")]
    public List<Media>? Evidences { get; set; } = Evidences;

    [JsonPropertyName("acceptCancellationReasons")]
    public List<string>? AcceptCancellationReasons { get; init; } = AcceptCancellationReasons;
}