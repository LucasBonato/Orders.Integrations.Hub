namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;

public record RappiOrderStore(
    string InternalId,
    string ExternalId,
    string Name
);