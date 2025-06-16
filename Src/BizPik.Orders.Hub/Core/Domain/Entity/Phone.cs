using System.Text.Json.Serialization;

namespace BizPik.Orders.Hub.Core.Domain.Entity;

public record Phone(
    [property: JsonPropertyName("number")] string Number,
    [property: JsonPropertyName("extension")] string Extension
);