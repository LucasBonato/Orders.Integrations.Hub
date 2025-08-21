namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Entity.Order;

public record AdditionalInfo(
    Dictionary<string, string> Metadata
);