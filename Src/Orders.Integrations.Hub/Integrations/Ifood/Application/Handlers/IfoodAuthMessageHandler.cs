using System.Net.Http.Headers;

using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Ifood.Application.Clients;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Handlers;

public class IfoodAuthMessageHandler(
    ILogger<IfoodAuthMessageHandler> logger,
    IfoodAuthClient ifoodAuthClient,
    ICacheService memoryCache
) : DelegatingHandler {
    private readonly string CACHE_KEY = AppEnv.INTEGRATIONS.IFOOD.CACHE.KEY.NotNullEnv();

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not null) {
            return await base.SendAsync(request, cancellationToken);
        }

        string accessToken = await memoryCache.GetOrSetTokenAsync(
               CACHE_KEY,
               nameof(IfoodAuthMessageHandler),
               logger,
               async () => {
                   IfoodAuthTokenResponse token = await ifoodAuthClient.RetrieveToken(
                       new IfoodAuthTokenRequest(
                           GrantType: "client_credentials",
                           ClientId: AppEnv.INTEGRATIONS.IFOOD.CLIENT.ID.NotNullEnv(),
                           ClientSecret: AppEnv.INTEGRATIONS.IFOOD.CLIENT.SECRET.NotNullEnv()
                       )
                   );

                   return (token.AccessToken, TimeSpan.FromSeconds(token.ExpiresIn));
               }
        );

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}