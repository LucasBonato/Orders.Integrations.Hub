using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record Media(
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("contentType")] string ContentType
);