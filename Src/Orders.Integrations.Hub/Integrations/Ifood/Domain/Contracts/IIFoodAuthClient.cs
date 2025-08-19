using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Ifood.Domain.Contracts;

public interface IIFoodAuthClient : IIntegrationAuthClient<IfoodAuthTokenRequest, IfoodAuthTokenResponse>;