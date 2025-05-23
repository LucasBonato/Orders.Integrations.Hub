using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Orders.Domain.Entity;

public record Phone(
    [property: JsonPropertyName("number")] string Number,
    [property: JsonPropertyName("extension")] string Extension
);