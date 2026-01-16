namespace Orders.Integrations.Hub.Core.Application.DTOs.Internal;

public record IntegrationResponse(
    int? TenantId,
    int? IntegrationId,
    IReadOnlyList<IntegrationSetting> Settings
);