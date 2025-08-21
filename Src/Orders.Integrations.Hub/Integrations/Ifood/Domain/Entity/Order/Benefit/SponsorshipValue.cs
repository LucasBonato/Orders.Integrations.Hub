using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order.Benefit;

public record SponsorshipValue(
    SponsorshipName Name,
    decimal Value,
    string Description
);