using Microsoft.Extensions.Configuration;

using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;

/// <summary>
/// Supplies deterministic integration settings for the application's current in-code
/// InternalClient. It is not an HTTP mock: InternalClient does not perform HTTP today.
/// If that adapter becomes a real HTTP client, replace this adapter with a WireMock API.
/// </summary>
internal sealed class TestIntegrationClient(
    IConfiguration configuration, 
    bool autoAccept
) : IInternalClient {
    private const string IFoodMerchant = "ifood-merchant";
    private const string RappiStore = "12345";
    private const string Food99Shop = "food99-shop";

    private readonly IntegrationResponse _settings = new(
        TenantId: 1,
        IntegrationId: 1,
        Settings: [
            new IntegrationSetting("ifood_merchant_id", IFoodMerchant),
            new IntegrationSetting("ifood_client_id", configuration["Integrations:IFood:Client:Id"] ?? "ifood-test-id"),
            new IntegrationSetting("ifood_client_secret", configuration["Integrations:IFood:Client:Secret"] ?? "ifood-test-secret"),
            new IntegrationSetting("ifood_mode", nameof(IntegrationMode.Centralized)),
            new IntegrationSetting("rappi_store_id", RappiStore),
            new IntegrationSetting("rappi_client_id", configuration["Integrations:Rappi:Client:Id"] ?? "rappi-test-id"),
            new IntegrationSetting("rappi_client_secret", configuration["Integrations:Rappi:Client:Secret"] ?? "rappi-test-secret"),
            new IntegrationSetting("rappi_mode", nameof(IntegrationMode.Centralized)),
            new IntegrationSetting("99food_app_shop_id", Food99Shop),
            new IntegrationSetting("99food_client_id", configuration["Integrations:Food99:Client:Id"] ?? "food99-test-id"),
            new IntegrationSetting("99food_client_secret", configuration["Integrations:Food99:Client:Secret"] ?? "food99-test-secret"),
            new IntegrationSetting("99food_mode", nameof(IntegrationMode.Centralized)),
            new IntegrationSetting("enable_auto_accept", autoAccept.ToString())
        ]);

    public Task<IntegrationResponse> GetIntegrationByExternalId(string externalId)
        => Task.FromResult(_settings);

    public Task<IntegrationResponse?> TryGetAppLevelIntegration(string integrationKey)
        => Task.FromResult<IntegrationResponse?>(_settings);
}
