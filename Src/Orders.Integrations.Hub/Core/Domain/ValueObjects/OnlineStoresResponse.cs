using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core..Domain.ValueObjects;

public record OnlineStoresResponse(
    [property: JsonPropertyName("stores")] List<string> Stores
);