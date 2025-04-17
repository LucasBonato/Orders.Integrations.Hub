using System.Text.Json.Serialization;

using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.Entity.Benefit;

public record Benefit(
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("target")] [property: JsonConverter(typeof(JsonStringEnumConverter))] Target Target,
    [property: JsonPropertyName("targetId")] string TargetId,
    [property: JsonPropertyName("sponsorshipValues")] List<SponsorshipValue>? SponsorshipValues,
    [property: JsonPropertyName("campaign")] Campaign Campaign
);