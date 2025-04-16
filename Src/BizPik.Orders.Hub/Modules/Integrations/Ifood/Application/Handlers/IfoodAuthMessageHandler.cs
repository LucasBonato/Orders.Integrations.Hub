using BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Clients;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Request;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects.DTOs.Response;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Handlers;

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
                ClientId: AppEnv.INTEGRATIONS.IFOOD.CLIENT.ID.NotNull(),
                ClientSecret: AppEnv.INTEGRATIONS.IFOOD.CLIENT.SECRET.NotNull()
            )
        );

        logger.LogWarning("[INFO] - IfoodAuthMessageHandler - Generating new Access token");

        request.Headers.Authorization = new("Bearer", token.AccessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}