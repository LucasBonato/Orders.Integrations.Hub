using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Indoor(
    IndoorMode Mode,
    string Table,
    DateTime DeliveryDateTime,
    string Observations
);