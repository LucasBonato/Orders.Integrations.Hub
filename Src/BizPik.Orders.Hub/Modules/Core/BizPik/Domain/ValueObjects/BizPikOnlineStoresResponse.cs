using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;

public record BizPikOnlineStoresResponse(
    [property: JsonPropertyName("stores")] List<string> Stores
);