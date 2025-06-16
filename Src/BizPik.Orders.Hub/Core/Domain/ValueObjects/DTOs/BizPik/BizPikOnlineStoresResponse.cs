using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.ValueObjects.DTOs.BizPik;

public record BizPikOnlineStoresResponse(
    [property: JsonPropertyName("stores")] List<string> Stores
);