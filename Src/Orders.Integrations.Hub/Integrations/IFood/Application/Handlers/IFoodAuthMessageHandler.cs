using System.Net.Http.Headers;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.IFood.Application.Handlers;

public class IFoodAuthMessageHandler(
    ILogger<IFoodAuthMessageHandler> logger,
    IIFoodAuthClient iFoodAuthClient,
    ICacheService cacheService
) : DelegatingHandler {
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not null) {
            return await base.SendAsync(request, cancellationToken);
        }

        IIntegrationContext integrationContext = request.GetIntegrationContext();

        IntegrationResolved integration = integrationContext.Integration ?? throw new NullReferenceException("integrationContext.Integration");
        string merchantId = integrationContext.MerchantId ?? throw new NullReferenceException("integrationContext.merchantId");

        string cacheKey = $"ifood-token:{integrationContext.TenantId}:{merchantId}";

        string accessToken = await cacheService.GetOrSetTokenAsync(
            cacheKey,
            nameof(IFoodAuthMessageHandler),
            logger,
            async () => {
                IFoodAuthTokenResponse token = await iFoodAuthClient.RetrieveToken(
                   new IFoodAuthTokenRequest(
                       GrantType: "client_credentials",
                       ClientId: integration.ClientId,
                       ClientSecret: integration.ClientSecret
                   )
               );

               return (token.AccessToken, TimeSpan.FromSeconds(token.ExpiresIn));
            }
        );

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}