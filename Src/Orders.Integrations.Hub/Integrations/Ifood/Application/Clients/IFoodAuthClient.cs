using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Clients;

public class IFoodAuthClient(
    [FromKeyedServices(OrderIntegration.IFOOD)] ICustomJsonSerializer jsonSerializer,
    ILogger<IFoodAuthClient> logger,
    HttpClient httpClient
) : IIFoodAuthClient {
    public async Task<IfoodAuthTokenResponse> RetrieveToken(IfoodAuthTokenRequest request)
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
        return jsonSerializer.Deserialize<IfoodAuthTokenResponse>(responseContent)!;
    }

    public Task<IfoodAuthTokenResponse> RefreshToken(IfoodAuthTokenRequest request) => throw new NotImplementedException();
}