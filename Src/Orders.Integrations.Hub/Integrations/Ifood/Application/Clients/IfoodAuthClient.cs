using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Clients;

public class IfoodAuthClient(
    ILogger<IfoodAuthClient> logger,
    HttpClient httpClient,
    ICustomJsonSerializer serializer
) : IIntegrationAuthClient<IfoodAuthTokenRequest, IfoodAuthTokenResponse> {
    public async Task<IfoodAuthTokenResponse> RetrieveToken(IfoodAuthTokenRequest request)
    {
        Dictionary<string, string> form = new() {
            { "grantType", request.GrantType },
            { "clientId", request.ClientId },
            { "clientSecret", request.ClientSecret }
        };

        const string uri = "authentication/v1.0/oauth/token";

        HttpRequestMessage requestMessage = new(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(form) };

        logger.LogInformation("[INFO] - IfoodAuthClient - Uri: {uri}", requestMessage.RequestUri);

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        string responseContent = await response.Content.ReadAsStringAsync();
        return serializer.Deserialize<IfoodAuthTokenResponse>(responseContent)!;
    }
}