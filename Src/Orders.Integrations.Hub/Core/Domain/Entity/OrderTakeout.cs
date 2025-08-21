using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderTakeout(
    OrderTakeoutMode Mode,
    DateTime TakeoutDateTime
);