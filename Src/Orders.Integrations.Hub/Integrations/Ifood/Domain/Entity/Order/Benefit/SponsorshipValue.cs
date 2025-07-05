using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Benefit;

public record SponsorshipValue(
    [property: JsonPropertyName("name")] [property: JsonConverter(typeof(JsonStringEnumConverter))] SponsorshipName Name,
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("description")] string Description
);