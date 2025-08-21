namespace Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Internal;

public record IntegrationResponse(
    int? TenantId,
    int? IntegrationId,
    IReadOnlyList<IntegrationSetting> Settings
);