namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;

public record AdditionalInfo(
    Dictionary<string, string> Metadata
);