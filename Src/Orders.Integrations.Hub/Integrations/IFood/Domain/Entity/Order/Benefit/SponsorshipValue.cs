using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Benefit;

public record SponsorshipValue(
    SponsorshipName Name,
    decimal Value,
    string Description
);