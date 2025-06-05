using BizPik.Orders.Hub.Core.Orders.Application.Extensions;
using BizPik.Orders.Hub.Integrations.Ifood.Application.Clients;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Integrations.Ifood.Application.Handlers;

public class IfoodAuthMessageHandler(
    ILogger<IfoodAuthMessageHandler> logger,
    IfoodAuthClient ifoodAuthClient
) : DelegatingHandler {

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not null) {
            return await base.SendAsync(request, cancellationToken);
        }

        IfoodAuthTokenResponse token = await ifoodAuthClient.RetrieveToken(
            new IfoodAuthTokenRequest(
                GrantType: "client_credentials",
                ClientId: AppEnv.INTEGRATIONS.IFOOD.CLIENT.ID.NotNullEnv(),
                ClientSecret: AppEnv.INTEGRATIONS.IFOOD.CLIENT.SECRET.NotNullEnv()
            )
        );

        logger.LogWarning("[INFO] - IfoodAuthMessageHandler - Generating new Access token");

        request.Headers.Authorization = new("Bearer", token.AccessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}