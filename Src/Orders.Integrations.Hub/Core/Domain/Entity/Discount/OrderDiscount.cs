using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Discount;

public record OrderDiscount(
    Price Amount,
    DiscountTarget Target,
    string TargetId,
    IReadOnlyList<OrderDiscountSponsorshipValue> SponsorshipValues
);