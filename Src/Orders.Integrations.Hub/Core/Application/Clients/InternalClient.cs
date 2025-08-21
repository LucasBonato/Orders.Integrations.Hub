using Orders.Integrations.Hub.Core.Domain.Contracts.Clients;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Internal;

namespace Orders.Integrations.Hub.Core.Application.Clients;

public class InternalClient(
    ILogger<InternalClient> logger,
    HttpClient httpClient
) : IInternalClient {
    public Task<IntegrationResponse> GetIntegrationByExternalId(string externalId) {
        throw new NotImplementedException();
    }
}