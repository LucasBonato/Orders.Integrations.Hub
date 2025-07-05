namespace Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiIntegrationResolved(
    string RappiClientId,
    string RappiStoreId,
    string RappiClientSecret
);