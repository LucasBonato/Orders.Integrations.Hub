namespace BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

public record RappiIntegrationResolved(
    string RappiClientId,
    string RappiStoreId,
    string RappiClientSecret
);