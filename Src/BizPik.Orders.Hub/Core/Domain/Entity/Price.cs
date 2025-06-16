using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.Entity;

public record Price(
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("currency")] string Currency
);