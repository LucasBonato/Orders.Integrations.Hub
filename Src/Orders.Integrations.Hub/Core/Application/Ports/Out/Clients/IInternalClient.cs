using Orders.Integrations.Hub.Core.Application.DTOs.Internal;

namespace Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;

public interface IInternalClient
{
    Task<IntegrationResponse> GetIntegrationByExternalId(string externalId);
}