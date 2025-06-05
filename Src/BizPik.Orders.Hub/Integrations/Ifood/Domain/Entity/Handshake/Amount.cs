using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Handshake;

public record Amount(
    [property: JsonPropertyName("value")] string Value,
    [property: JsonPropertyName("currency")] string Currency
);