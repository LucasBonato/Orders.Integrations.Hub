using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Discount;

public record OrderDiscountSponsorshipValue(
    OrderSponsorshipName Name,
    Price Amount,
    string? Description
);