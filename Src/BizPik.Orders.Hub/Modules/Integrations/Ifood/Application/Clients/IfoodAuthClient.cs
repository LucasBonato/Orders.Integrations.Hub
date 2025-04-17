using System.Text.Json;
using BizPik.Orders.Hub.Modules.Integrations.Common.Domain.Contracts;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Clients;

public class IfoodAuthClient(
    ILogger<IfoodAuthClient> logger,
    HttpClient httpClient
) : IIntegrationAuthClient<IfoodAuthTokenRequest, IfoodAuthTokenResponse> {
    public async Task<IfoodAuthTokenResponse> RetrieveToken(IfoodAuthTokenRequest request)
    {
        Dictionary<string, string> form = new() {
            { "grantType", request.GrantType },
            { "clientId", request.ClientId },
            { "clientSecret", request.ClientSecret }
        };

        string uri = AppEnv.INTEGRATIONS.IFOOD.ENDPOINT.AUTH.NotNull();

        HttpRequestMessage requestMessage = new(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(form) };

        logger.LogInformation("[INFO] - IfoodAuthClient - Uri: {uri}", requestMessage.RequestUri);

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

        string responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IfoodAuthTokenResponse>(responseContent)!;
    }
}