using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Clients;

public class IFoodAuthClient(
    [FromKeyedServices(IFoodIntegrationKey.Value)] ICustomJsonSerializer jsonSerializer,
    ILogger<IFoodAuthClient> logger,
    HttpClient httpClient
) : IIFoodAuthClient {
    public async Task<IFoodAuthTokenResponse> RetrieveToken(IFoodAuthTokenRequest request)
    {
        Dictionary<string, string> form = new() {
            { "grantType", request.GrantType },
            { "clientId", request.ClientId },
            { "clientSecret", request.ClientSecret }
        };

        const string uri = "authentication/v1.0/oauth/token";

        HttpRequestMessage requestMessage = new(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(form) };

        logger.LogInformation("[INFO] - IFoodAuthClient - Uri: {uri}", requestMessage.RequestUri);

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        string responseContent = await response.Content.ReadAsStringAsync();
        return jsonSerializer.Deserialize<IFoodAuthTokenResponse>(responseContent)!;
    }

    public Task<IFoodAuthTokenResponse> RefreshToken(IFoodAuthTokenRequest request) => throw new NotImplementedException();
}