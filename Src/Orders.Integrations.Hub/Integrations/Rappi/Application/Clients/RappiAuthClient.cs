using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Clients;

public class RappiAuthClient(
    ILogger<RappiAuthClient> logger,
    HttpClient httpClient,
    ICustomJsonSerializer jsonSerializer
) : IIntegrationAuthClient<RappiAuthTokenRequest, RappiAuthTokenResponse> {
    public async Task<RappiAuthTokenResponse> RetrieveToken(RappiAuthTokenRequest request)
    {
        string uri = "oauth/token";

        logger.LogInformation("[INFO] - IfoodAuthClient - Uri: {uri}", uri);

        HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(jsonSerializer.Serialize(request)));

        string responseContent = await response.Content.ReadAsStringAsync();
        return jsonSerializer.Deserialize<RappiAuthTokenResponse>(responseContent)!;
    }
}