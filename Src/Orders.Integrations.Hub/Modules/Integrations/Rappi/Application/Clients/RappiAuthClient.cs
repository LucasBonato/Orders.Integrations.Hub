using System.Text.Json;
using Orders.Integrations.Hub.Modules.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Modules.Integrations.Rappi.Domain.DTOs.Request;
using Orders.Integrations.Hub.Modules.Integrations.Rappi.Domain.DTOs.Response;

namespace Orders.Integrations.Hub.Modules.Integrations.Rappi.Application.Clients;

public class RappiAuthClient(
    ILogger<RappiAuthClient> logger,
    HttpClient httpClient
) : IIntegrationAuthClient<RappiAuthTokenRequest, RappiAuthTokenResponse> {
    public async Task<RappiAuthTokenResponse> RetrieveToken(RappiAuthTokenRequest request)
    {
        string uri = "/oauth/token";

        logger.LogInformation("[INFO] - IfoodAuthClient - Uri: {uri}", uri);

        HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(JsonSerializer.Serialize(request)));

        string responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RappiAuthTokenResponse>(responseContent)!;
    }
}