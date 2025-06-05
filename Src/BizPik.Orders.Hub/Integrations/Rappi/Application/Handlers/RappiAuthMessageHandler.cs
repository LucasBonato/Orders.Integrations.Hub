using BizPik.Orders.Hub.Core.Orders.Application.Extensions;
using BizPik.Orders.Hub.Integrations.Rappi.Application.Clients;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Integrations.Rappi.Application.Handlers;

public class RappiAuthMessageHandler(
    ILogger<RappiAuthMessageHandler> logger,
    RappiAuthClient rappiAuthClient
) : DelegatingHandler {
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not null) {
            return await base.SendAsync(request, cancellationToken);
        }

        RappiAuthTokenResponse token = await rappiAuthClient.RetrieveToken(
            new RappiAuthTokenRequest(
                ClienteId: AppEnv.INTEGRATIONS.RAPPI.CLIENT.ID.NotNullEnv(),
                ClienteSecret: AppEnv.INTEGRATIONS.RAPPI.CLIENT.SECRET.NotNullEnv(),
                Audience: AppEnv.INTEGRATIONS.RAPPI.CLIENT.AUDIENCE.NotNullEnv()
            )
        );

        logger.LogWarning("[INFO] - RappiAuthMessageHandler - Generating new Access token");

        request.Headers.Add("x-authorization", $"{token.TokenType} {token.AccessToken}");

        return await base.SendAsync(request, cancellationToken);
    }
}