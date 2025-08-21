using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Internal;

namespace Orders.Integrations.Hub.Core.Domain.Contracts.Clients;

public interface IInternalClient
{
    Task<IntegrationResponse> GetIntegrationByExternalId(string externalId);
}