using BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Clients;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Modules.Integrations.Rappi.Application.Handlers;

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
                ClienteId: AppEnv.INTEGRATIONS.RAPPI.CLIENT.ID.NotNull(),
                ClienteSecret: AppEnv.INTEGRATIONS.RAPPI.CLIENT.SECRET.NotNull(),
                Audience: AppEnv.INTEGRATIONS.RAPPI.CLIENT.AUDIENCE.NotNull()
            )
        );

        logger.LogWarning("[INFO] - RappiAuthMessageHandler - Generating new Access token");

        request.Headers.Add("x-authorization", $"{token.TokenType} {token.AccessToken}");

        return await base.SendAsync(request, cancellationToken);
    }
}