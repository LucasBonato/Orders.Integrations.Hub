using System.Collections.Specialized;
using System.Net;
using System.Web;

using Orders.Integrations.Hub.Core.Application.Ports.Out.Cache;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Core.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Food99.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Request;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Response;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Handlers;

public class Food99AuthMessageHandler(
    [FromKeyedServices(Food99IntegrationKey.Value)] ICustomJsonSerializer jsonSerializer,
    ILogger<Food99AuthMessageHandler> logger,
    IFood99AuthClient food99AuthClient,
    ICacheService cacheService
) : DelegatingHandler {
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not null) {
            return await base.SendAsync(request, cancellationToken);
        }

        IIntegrationContext integrationContext = request.GetIntegrationContext();

        IntegrationResolved integration = integrationContext.Integration ?? throw new NullReferenceException("integrationContext.Integration");
        string merchantId = integrationContext.MerchantId ?? throw new NullReferenceException("integrationContext.MerchantId");

        string cacheKey = $"food99-token:{integration.TenantId}:{merchantId}";

        string accessToken = await cacheService.GetOrSetTokenAsync(
            cacheKey,
            nameof(Food99AuthMessageHandler),
            logger,
            async () => await RetrieveTokenAsync(integration)
        );

        // This is bullshit, for some reason the 99Food needs the token in the body or the query params
        await InjectTokenIntoRequest(request, accessToken);

        logger.LogInformation(
            "[INFO] Injected token: {Token} into {Method} {Uri} {body}",
            accessToken,
            request.Method,
            request.RequestUri,
            (request.Method == HttpMethod.Post) ? await request.Content?.ReadAsStringAsync(cancellationToken)! : null
        );

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
            return response;

        Food99BaseResponse responseBody = jsonSerializer.Deserialize<Food99BaseResponse>(await response.Content.ReadAsStringAsync(cancellationToken))
                                          ?? throw new InvalidOperationException("Response body is null");

        if (responseBody.Errno == (int)Food99Errno.AuthTokenHasExpired)
        {
            logger.LogWarning("[WARN] - {context} - Token expired, trying to refresh...", nameof(Food99AuthMessageHandler));

            (accessToken, TimeSpan expiration) = await RetrieveTokenAsync(integration);

            await cacheService.SetAsync(cacheKey, accessToken, expiration);
            await InjectTokenIntoRequest(request, accessToken);
            logger.LogInformation("[INFO] Injected token: {Token} into {Method} {Uri} {body}", accessToken, request.Method, request.RequestUri, (request.Method == HttpMethod.Post) ? await request.Content?.ReadAsStringAsync(cancellationToken)! : null);
        }

        response = await base.SendAsync(request, cancellationToken);

        return response;
    }

    private async Task<(string AccessToken, TimeSpan Expiration)> RetrieveTokenAsync(IntegrationResolved integration)
    {
        string appId = integration.ClientId;
        string appSecret = integration.ClientSecret;
        string merchantId = integration.MerchantId;

        Food99AuthTokenResponse token = await GetTokenAsync(appId, appSecret, merchantId);

        if (token.Errno == (int)Food99Errno.AuthTokenHasExpired) {
            await RefreshTokenAsync(appId, appSecret, merchantId);
            token = await GetTokenAsync(appId, appSecret, merchantId);
        }

        return (token.Data!.AuthToken, GetExpirationTime(token.Data.TokenExpirationTime));
    }

    private async Task<Food99AuthTokenResponse> GetTokenAsync(string appId, string appSecret, string merchantId)
    {
        return await food99AuthClient.RetrieveToken(
            new Food99AuthTokenRequest(
                AppId: appId,
                AppSecret: appSecret,
                AppShopId: merchantId
            )
        );
    }

    private async Task RefreshTokenAsync(string appId, string appSecret,  string merchantId)
    {
        await food99AuthClient.RefreshToken(
            new Food99AuthTokenRequest(
                AppId: appId,
                AppSecret: appSecret,
                AppShopId: merchantId
            )
        );
    }

    private static TimeSpan GetExpirationTime(long expirationTime) {
        DateTimeOffset expirationDate = DateTimeOffset.FromUnixTimeSeconds(expirationTime);
        TimeSpan expiresIn = expirationDate - DateTimeOffset.UtcNow;

        if (expiresIn < TimeSpan.Zero)
            expiresIn = TimeSpan.Zero;
        return expiresIn;
    }

    private async Task InjectTokenIntoRequest(HttpRequestMessage request, string accessToken)
    {
        if (request.Method == HttpMethod.Get) {
            InjectTokenIntoQuery(request, accessToken);
        }
        else {
            await InjectTokenIntoBodyAsync(request, accessToken);
        }
    }

    private async Task InjectTokenIntoBodyAsync(HttpRequestMessage request, string accessToken)
    {
        if (request.Content is null)
            throw new InvalidOperationException("Could not inject token, request has no body.");

        await request.Content.LoadIntoBufferAsync();
        string body = await request.Content.ReadAsStringAsync();

        Food99StatusChangeRequest dto = jsonSerializer.Deserialize<Food99StatusChangeRequest>(body)
                                        ?? throw new InvalidOperationException("Could not deserialize body to ConfirmOrderRequest.");

        Food99StatusChangeRequest newRequest = dto with { AuthToken = accessToken };

        string updatedBody = jsonSerializer.Serialize(newRequest);

        StringContent newContent = new(updatedBody);
        newContent.Headers.Clear();
        newContent.Headers.TryAddWithoutValidation("Content-Type", "application/json");

        request.Content = newContent;
    }

    private static void InjectTokenIntoQuery(HttpRequestMessage request, string accessToken) {
        UriBuilder uriBuilder = new(request.RequestUri!);
        NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query.Set("auth_token", accessToken);
        uriBuilder.Query = query.ToString();
        request.RequestUri = uriBuilder.Uri;
    }
}