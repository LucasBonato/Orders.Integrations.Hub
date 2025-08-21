using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record Takeout(
    TakeoutMode Mode,
    DateTime TakeoutDateTime,
    string Observations
);