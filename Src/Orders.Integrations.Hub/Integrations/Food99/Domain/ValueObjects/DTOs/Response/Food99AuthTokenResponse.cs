using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs.Response;

public enum Food99Errno
{
    NaoFoiPossivelAtualizarAsInformacoesDoPedido = 10001,
    ParameterError = 10002,
    PermissionDenied = 10006,
    GetAuthTokenFailed = 10100,
    ThisShopDoesNotHaveAuthToken = 10101,
    AuthTokenHasExpired = 10102,
    GetAppFailed = 14103,
    AppIdDoesNotExist = 14105,
    AppSecretIsWrong = 14106
}

public record Food99AuthRefreshTokenResponse(
    [property: JsonPropertyName("errno")] int Errno,
    [property: JsonPropertyName("errmsg")] string Errmsg,
    [property: JsonPropertyName("requestId")] string RequestId,
    [property: JsonPropertyName("time")] long Time,
    [property: JsonPropertyName("data")] bool Data
);

public record Food99AuthTokenResponse(
    [property: JsonPropertyName("errno")] int Errno,
    [property: JsonPropertyName("errmsg")] string Errmsg,
    [property: JsonPropertyName("requestId")] string RequestId,
    [property: JsonPropertyName("time")] long Time,
    [property: JsonPropertyName("data")] Food99AuthDataTokenResponse? Data
);

public record Food99AuthDataTokenResponse(
    [property: JsonPropertyName("app_id")] long AppId,
    [property: JsonPropertyName("app_shop_id")] string AppShopId,
    [property: JsonPropertyName("auth_token")] string AuthToken,
    [property: JsonPropertyName("token_expiration_time")] long TokenExpirationTime
);