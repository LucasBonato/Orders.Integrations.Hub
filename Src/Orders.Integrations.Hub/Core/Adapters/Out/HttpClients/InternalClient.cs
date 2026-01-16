using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

namespace Orders.Integrations.Hub.Core.Adapters.Out.HttpClients;

public class InternalClient(
    ILogger<InternalClient> logger,
    HttpClient httpClient
) : IInternalClient {
    public Task<IntegrationResponse> GetIntegrationByExternalId(string externalId) {
        throw new NotImplementedException();
    }
}