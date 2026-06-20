using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Application.ValueObjects;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Clients;

public class RappiAuthClient(
    [FromKeyedServices(RappiIntegrationKey.Value)] ICustomJsonSerializer jsonSerializer,
    ILogger<RappiAuthClient> logger,
    HttpClient httpClient
) : IRappiAuthClient {
    public async Task<RappiAuthTokenResponse> RetrieveToken(RappiAuthTokenRequest request)
    {
        const string uri = "oauth/token";

        logger.LogInformation("[INFO] - IFoodAuthClient - Uri: {uri}", uri);

        HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(jsonSerializer.Serialize(request)));

        string responseContent = await response.Content.ReadAsStringAsync();
        return jsonSerializer.Deserialize<RappiAuthTokenResponse>(responseContent)!;
    }

    public Task<RappiAuthTokenResponse> RefreshToken(RappiAuthTokenRequest request)
        => throw new NotImplementedException();
}