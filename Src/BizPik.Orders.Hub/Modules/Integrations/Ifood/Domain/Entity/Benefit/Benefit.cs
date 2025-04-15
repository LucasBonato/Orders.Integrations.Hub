using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.Entity.Benefit;

public record Benefit(
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("target")] Target Target,
    [property: JsonPropertyName("targetId")] string TargetId,
    [property: JsonPropertyName("sponsorshipValues")] List<SponsorshipValue>? SponsorshipValues,
    [property: JsonPropertyName("campaign")] Campaign Campaign
);