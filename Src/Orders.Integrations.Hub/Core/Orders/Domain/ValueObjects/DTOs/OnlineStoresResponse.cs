using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Core.Orders.Domain.ValueObjects.DTOs.;

public record OnlineStoresResponse(
    [property: JsonPropertyName("stores")] List<string> Stores
);