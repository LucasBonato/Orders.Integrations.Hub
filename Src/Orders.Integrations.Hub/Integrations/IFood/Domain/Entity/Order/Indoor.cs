using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;

public record Indoor(
    IndoorMode Mode,
    string Table,
    DateTime DeliveryDateTime,
    string Observations
);