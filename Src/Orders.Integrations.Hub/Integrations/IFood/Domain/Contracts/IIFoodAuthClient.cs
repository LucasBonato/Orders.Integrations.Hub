using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;

public interface IIFoodAuthClient : IIntegrationAuthClient<IFoodAuthTokenRequest, IFoodAuthTokenResponse>;