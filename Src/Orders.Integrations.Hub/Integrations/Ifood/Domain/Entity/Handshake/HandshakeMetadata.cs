using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record HandshakeMetadata(
    [property: JsonPropertyName("item")] List<Item>? Items,
    [property: JsonPropertyName("garnishItems")] List<GarnishItem>? GarnishItems,
    [property: JsonPropertyName("evidences")] List<Media>? Evidences,
    [property: JsonPropertyName("acceptCancellationReasons")] List<string>? AcceptCancellationReasons
);