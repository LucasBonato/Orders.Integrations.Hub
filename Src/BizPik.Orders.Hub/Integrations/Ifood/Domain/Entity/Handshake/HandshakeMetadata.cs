using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record HandshakeMetadata(
    [property: JsonPropertyName("item")] List<Item>? Items,
    [property: JsonPropertyName("garnishItems")] List<GarnishItem>? GarnishItems,
    [property: JsonPropertyName("evidences")] List<Media>? Evidences,
    [property: JsonPropertyName("acceptCancellationReasons")] List<NegotiationReasons>? AcceptCancellationReasons
);