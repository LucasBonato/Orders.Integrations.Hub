namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrder(
    RappiOrderDetails OrderDetail,
    RappiOrderCustomer? Customer,
    RappiOrderStore Store
);