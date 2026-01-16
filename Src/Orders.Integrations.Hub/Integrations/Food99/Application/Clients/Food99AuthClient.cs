using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Clients;

public class Food99AuthClient(
    [FromKeyedServices(Food99IntegrationKey.Value)]
    ICustomJsonSerializer jsonSerializer,
    ILogger<Food99AuthClient> logger,
    HttpClient httpClient
) : IFood99AuthClient
{
    public async Task<Food99AuthTokenResponse> RetrieveToken(Food99AuthTokenRequest request)
    {
        string uri = $"v1/auth/authtoken/get?app_id={request.AppId}&app_secret={request.AppSecret}&app_shop_id={request.AppShopId}";

        logger.LogInformation("[INFO] - Food99AuthClient/RetrieveToken - Uri: {uri}", uri);

        HttpResponseMessage response = await httpClient.GetAsync(uri);

        string responseContent = await response.Content.ReadAsStringAsync();
        return jsonSerializer.Deserialize<Food99AuthTokenResponse>(responseContent)!;
    }

    public async Task<Food99AuthTokenResponse> RefreshToken(Food99AuthTokenRequest request)
    {
        string uri = $"v1/auth/authtoken/refresh?app_id={request.AppId}&app_secret={request.AppSecret}&app_shop_id={request.AppShopId}";

        logger.LogInformation("[INFO] - Food99AuthClient/RefreshToken - Uri: {uri}", uri);

        HttpResponseMessage response = await httpClient.GetAsync(uri);

        string responseContent = await response.Content.ReadAsStringAsync();
        Food99AuthRefreshTokenResponse dataDeserialized = jsonSerializer.Deserialize<Food99AuthRefreshTokenResponse>(responseContent)
                                                           ?? throw new Exception("Deserialization failed");
        return new Food99AuthTokenResponse(
            Errno: dataDeserialized.Errno,
            Errmsg: dataDeserialized.Errmsg,
            RequestId: dataDeserialized.RequestId,
            Time: dataDeserialized.Time,
            Data: null
        );
    }    
}