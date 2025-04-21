namespace Orders.Integrations.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiIntegrationResolved(
    string RappiClientId,
    string RappiStoreId,
    string RappiClientSecret
);