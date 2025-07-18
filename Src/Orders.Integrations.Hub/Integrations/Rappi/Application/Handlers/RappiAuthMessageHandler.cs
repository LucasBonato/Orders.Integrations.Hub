﻿using Orders.Integrations.Hub.Core.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Clients;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Handlers;

public class RappiAuthMessageHandler(
    ILogger<RappiAuthMessageHandler> logger,
    RappiAuthClient rappiAuthClient,
    ICacheService cacheService
) : DelegatingHandler {
    private readonly string CACHE_KEY = AppEnv.INTEGRATIONS.RAPPI.CACHE.KEY.NotNullEnv();

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not null) {
            return await base.SendAsync(request, cancellationToken);
        }

        string accessToken = await cacheService.GetOrSetTokenAsync(
            CACHE_KEY,
            nameof(RappiAuthMessageHandler),
            logger,
            async () => {
                RappiAuthTokenResponse token = await rappiAuthClient.RetrieveToken(
                    new RappiAuthTokenRequest(
                        ClienteId: AppEnv.INTEGRATIONS.RAPPI.CLIENT.ID.NotNullEnv(),
                        ClienteSecret: AppEnv.INTEGRATIONS.RAPPI.CLIENT.SECRET.NotNullEnv(),
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