using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderIndoor(
    OrderIndoorMode Mode,
    DateTime IndoorDateTime,
    string? Place,
    string? Seat,
    string? Tab
);