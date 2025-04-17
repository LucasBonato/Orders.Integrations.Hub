using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Benefit;

public record SponsorshipValue(
    [property: JsonPropertyName("name")] [property: JsonConverter(typeof(JsonStringEnumConverter))] SponsorshipName Name,
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("description")] string Description
);