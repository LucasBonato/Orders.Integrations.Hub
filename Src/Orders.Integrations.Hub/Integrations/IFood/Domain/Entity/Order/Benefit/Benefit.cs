using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Benefit;

public record Benefit(
    decimal Value,
    Target Target,
    string? TargetId,
    List<SponsorshipValue>? SponsorshipValues,
    Campaign Campaign
);