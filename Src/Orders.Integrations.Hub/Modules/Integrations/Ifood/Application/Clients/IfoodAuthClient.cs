using System.Text.Json;
using Orders.Integrations.Hub.Modules.Integrations.Common.Domain.Contracts;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Clients;

public class IfoodAuthClient(
    HttpClient httpClient
) : IIntegrationAuthClient<IfoodAuthTokenRequest, IfoodAuthTokenResponse> {
    public async Task<IfoodAuthTokenResponse> RetrieveToken(IfoodAuthTokenRequest request)
    {
        Dictionary<string, string> form = new() {
            { "grant_type", request.GrantType },
            { "client_id", request.ClientId },
            { "client_secret", request.ClientSecret }
        };

        HttpRequestMessage requestMessage = new(HttpMethod.Post, AppEnv.INTEGRATIONS.IFOOD.ENDPOINT.AUTH.NotNull()) { Content = new FormUrlEncodedContent(form) };

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IfoodAuthTokenResponse>(responseContent)!;
    }
}