using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Core.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Core.Domain.Entity;

public record OrderTakeout(
    [property: JsonPropertyName("mode")] [property: JsonConverter(typeof(JsonStringEnumConverter))] OrderTakeoutMode Mode,
    [property: JsonPropertyName("takeoutDateTime")] DateTime TakeoutDateTime
);