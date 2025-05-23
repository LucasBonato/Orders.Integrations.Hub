using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Handshake;

public record Media(
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("contentType")] string ContentType
);