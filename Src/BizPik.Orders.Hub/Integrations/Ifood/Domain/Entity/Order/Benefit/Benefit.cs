using System.Text.Json.Serialization;

using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace BizPik.Orders.Hub.Integrations.Ifood.Domain.Entity.Order.Benefit;

public record Benefit(
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("target")] [property: JsonConverter(typeof(JsonStringEnumConverter))] Target Target,
    [property: JsonPropertyName("targetId")] string? TargetId,
    [property: JsonPropertyName("sponsorshipValues")] List<SponsorshipValue>? SponsorshipValues,
    [property: JsonPropertyName("campaign")] Campaign Campaign
);