using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Benefit;

public record Benefit(
    decimal Value,
    Target Target,
    string? TargetId,
    List<SponsorshipValue>? SponsorshipValues,
    Campaign Campaign
);