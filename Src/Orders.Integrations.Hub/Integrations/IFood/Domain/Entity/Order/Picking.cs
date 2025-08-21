using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;

public record Picking(
    string Picker,
    ReplacementOptions ReplacementOptions
);