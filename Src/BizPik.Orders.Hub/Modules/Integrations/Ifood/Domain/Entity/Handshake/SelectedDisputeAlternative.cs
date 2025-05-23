using System.Text.Json.Serialization;

using Type = BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums.Handshake.Type;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Handshake;

public record SelectedDisputeAlternative(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] Type Type,
    [property: JsonPropertyName("metadata")] DisputeAlternativeMetadata Metadata
);