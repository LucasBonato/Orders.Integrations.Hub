namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderTotal(
    Price ItemsPrice,
    Price OtherFees,
    Price? Discount,
    Price OrderAmount
);