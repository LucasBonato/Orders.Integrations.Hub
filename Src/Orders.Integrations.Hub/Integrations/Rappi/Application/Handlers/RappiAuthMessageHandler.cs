using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Core.Infrastructure.Extensions;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Clients;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Handlers;

public class RappiAuthMessageHandler(
    ILogger<RappiAuthMessageHandler> logger,
    RappiAuthClient rappiAuthClient,
    ICacheService cacheService
) : DelegatingHandler {
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not null) {
            return await base.SendAsync(request, cancellationToken);
        }

        IIntegrationContext integrationContext = request.GetIntegrationContext();

        IntegrationResolved integration = integrationContext.Integration ?? throw new NullReferenceException("integrationContext.Integration");

        string cacheKey = $"rappi-token:{integration.TenantId}:{integration.MerchantId}";

        string accessToken = await cacheService.GetOrSetTokenAsync(
            cacheKey,
            nameof(RappiAuthMessageHandler),
            logger,
            async () => {
                RappiAuthTokenResponse token = await rappiAuthClient.RetrieveToken(
                    new RappiAuthTokenRequest(
                        ClienteId: integration.ClientId,
                        ClienteSecret: integration.ClientSecret,
                        Audience: AppEnv.INTEGRATIONS.RAPPI.CLIENT.AUDIENCE.NotNullEnv()
                    )
                );

                return (token.AccessToken, TimeSpan.FromSeconds(token.ExpiresIn));
            }
        );

        request.Headers.Add("x-authorization", $"Bearer {accessToken}");

        return await base.SendAsync(request, cancellationToken);
    }
}