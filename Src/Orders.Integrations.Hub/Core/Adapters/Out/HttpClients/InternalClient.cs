using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;

namespace Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;

public class InternalClient(
    ILogger<InternalClient> logger
) : IInternalClient {
    public Task<IntegrationResponse> GetIntegrationByExternalId(string externalId) {
        if (logger.IsEnabled(LogLevel.Information)) {
            logger.LogInformation("Getting integration by external id {externalId}", externalId);
            logger.LogInformation("Mocked integration settings");
        }
        
        return Task.FromResult(
            new IntegrationResponse(
                TenantId: 1,
                IntegrationId: 1,
                Settings: [
                    new IntegrationSetting(
                        Name: "ifood_merchant_id",
                        Value: "dd2aad76-35aa-4816-952c-9194822155f8"
                    ),
                    new IntegrationSetting(
                        Name: "ifood_client_id",
                        Value: AppEnv.INTEGRATIONS.IFOOD.CLIENT.ID.NotNullEnv()
                    ),
                    new IntegrationSetting(
                        Name: "ifood_client_secret",
                        Value: AppEnv.INTEGRATIONS.IFOOD.CLIENT.SECRET.NotNullEnv()
                    )
                ]
            )
        );
    }
}