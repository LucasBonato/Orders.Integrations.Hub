using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Discount;

public record OrderDiscountSponsorshipValue(
    OrderSponsorshipName Name,
    Price Amount,
    string? Description
);