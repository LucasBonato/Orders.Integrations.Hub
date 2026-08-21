using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;

namespace Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;

public class InternalClient(
    ILogger<InternalClient> logger,
    IConfiguration configuration,
    HttpClient httpClient
) : IInternalClient {
    private readonly List<IntegrationResponse> _responses = [
        new(
            TenantId: 1,
            IntegrationId: 1,
            Settings:
            [
                new IntegrationSetting(
                    Name: "ifood_merchant_id",
                    Value: "dd2aad76-35aa-4816-952c-9194822155f8"
                ),
                new IntegrationSetting(
                    Name: "ifood_client_id",
                    Value: configuration.GetValue("Integrations:IFood:Client:Id", string.Empty)
                ),
                new IntegrationSetting(
                    Name: "ifood_client_secret",
                    Value: configuration.GetValue("Integrations:IFood:Client:Secret", string.Empty)
                ),
                new IntegrationSetting(
                    Name: "ifood_mode",
                    Value: nameof(IntegrationMode.Centralized)
                )
            ]
        ),
        new(
            TenantId: 1,
            IntegrationId: 2,
            Settings:
            [
                new IntegrationSetting(
                    Name: "rappi_merchant_id",
                    Value: "dd2aad76-35aa-4816-952c-9194822155f8"
                ),
                new IntegrationSetting(
                    Name: "rappi_client_id",
                    Value: configuration.GetValue("Integrations:Rappi:Client:Id", string.Empty)
                ),
                new IntegrationSetting(
                    Name: "rappi_client_secret",
                    Value: configuration.GetValue("Integrations:Rappi:Client:Id", string.Empty)
                ),
                new IntegrationSetting(
                    Name: "rappi_mode",
                    Value: nameof(IntegrationMode.Centralized)
                )
            ]
        ),
        new(
            TenantId: 1,
            IntegrationId: 3,
            Settings:
            [
                new IntegrationSetting(
                    Name: "food99_merchant_id",
                    Value: "dd2aad76-35aa-4816-952c-9194822155f8"
                ),
                new IntegrationSetting(
                    Name: "food99_client_id",
                    Value: configuration.GetValue("Integrations:Food99:Client:Id", string.Empty)
                ),
                new IntegrationSetting(
                    Name: "food99_client_secret",
                    Value: configuration.GetValue("Integrations:Food99:Client:Id", string.Empty)
                ),
                new IntegrationSetting(
                    Name: "food99_mode",
                    Value: nameof(IntegrationMode.Centralized)
                )
            ]
        )
    ];
    
    public Task<IntegrationResponse> GetIntegrationByExternalId(string externalId) {
        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("Getting integration by external id {externalId}", externalId);
            logger.LogInformation("Mocked integration settings");
        }

        string? _ = httpClient.BaseAddress?.AbsolutePath;

        IntegrationResponse integration = _responses.First();
        foreach (IntegrationResponse response in _responses) {
            bool hasExternalId = response.Settings.Any(setting => setting.Value == externalId);

            if (!hasExternalId)
                continue;

            integration = response;
            break;
        }
        
        return Task.FromResult(
            integration
        );
    }

    public Task<IntegrationResponse?> TryGetAppLevelIntegration(string integrationKey)
    {
        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("Getting integration level by integration key {integrationKey}", integrationKey);
            logger.LogInformation("Mocked integration settings");
        }
        
        IntegrationResponse? integration = null;
        foreach (IntegrationResponse response in _responses) {
            bool hasExternalId = response.Settings.Any(setting => setting.Name.Contains(integrationKey, StringComparison.InvariantCultureIgnoreCase));

            if (!hasExternalId)
                continue;

            integration = response;
            break;
        }
        
        return Task.FromResult(integration);
    }
}