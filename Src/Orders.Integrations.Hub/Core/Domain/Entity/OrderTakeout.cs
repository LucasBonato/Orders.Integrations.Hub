using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderTakeout(
    OrderTakeoutMode Mode,
    DateTime TakeoutDateTime
);