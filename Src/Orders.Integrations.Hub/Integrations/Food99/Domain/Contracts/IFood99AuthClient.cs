using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Food99.Domain.Contracts;

public interface IFood99AuthClient : IIntegrationAuthClient<Food99AuthTokenRequest, Food99AuthTokenResponse>;