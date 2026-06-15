using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderIndoor(
    OrderIndoorMode Mode,
    DateTime IndoorDateTime,
    string? Place,
    string? Seat,
    string? Tab
);