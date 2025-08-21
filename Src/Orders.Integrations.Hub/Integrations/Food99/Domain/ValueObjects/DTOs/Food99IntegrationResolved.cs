namespace Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs;

public record Food99IntegrationResolved(
    string Food99AppShopId,
    bool AutoAccept
);