using System.Security.Cryptography;
using System.Text;

using Orders.Integrations.Hub.Integrations.Rappi.Application.Handlers;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure;

public sealed class ScenarioResult(
    HttpResponseMessage response
) {
    public HttpResponseMessage Response { get; } = response;
}

public sealed class WebhookScenarioBuilder {
    private readonly HttpClient _httpClient;
    private readonly string _headerName;
    private string _payload = string.Empty;
    private string _route = string.Empty;
    private string _signature;

    private WebhookScenarioBuilder(HttpClient httpClient, string headerName, string signature) {
        _httpClient = httpClient;
        _headerName = headerName;
        _signature = signature;
    }

    public static WebhookScenarioBuilder ForIFood(HttpClient httpClient)
        => new(httpClient, "X-IFood-Signature", string.Empty);

    public static WebhookScenarioBuilder ForRappi(HttpClient httpClient)
        => new(httpClient, "Rappi-Signature", string.Empty);

    public static WebhookScenarioBuilder ForFood99(HttpClient httpClient)
        => new(httpClient, "didi-header-sign", string.Empty);

    public WebhookScenarioBuilder WithRoute(string route) {
        _route = route;
        return this;
    }

    public WebhookScenarioBuilder WithPayload(string payload) {
        _payload = payload;
        return this;
    }

    public WebhookScenarioBuilder WithSignature(string secret) {
        _signature = _headerName switch {
            "X-IFood-Signature" => ComputeIFoodSignature(secret),
            "Rappi-Signature" => ComputeRappiSignature(secret),
            "didi-header-sign" => ComputeFood99Signature(secret),
            _ => string.Empty
        };
        return this;
    }

    public WebhookScenarioBuilder WithoutSignature() {
        _signature = string.Empty;
        return this;
    }

    public async Task<ScenarioResult> PostAsync() {
        StringContent content = new(_payload, Encoding.UTF8, "application/json");

        if (!string.IsNullOrEmpty(_signature))
            content.Headers.TryAddWithoutValidation(_headerName, _signature);

        HttpResponseMessage response = await _httpClient.PostAsync(_route, content);
        return new ScenarioResult(response);
    }

    private string ComputeIFoodSignature(string secret) {
        byte[] hash = HMACSHA256.HashData(
            Encoding.UTF8.GetBytes(secret),
            Encoding.UTF8.GetBytes(_payload)
        );
        return Convert.ToHexStringLower(hash);
    }

    private string ComputeRappiSignature(string secret) {
        RappiJsonSerializer serializer = new();
        string reserialized = serializer.Serialize(serializer.Deserialize<object>(_payload));
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string payloadToSign = $"{timestamp}.{reserialized}";
        byte[] hash = HMACSHA256.HashData(
            Encoding.UTF8.GetBytes(secret),
            Encoding.UTF8.GetBytes(payloadToSign)
        );
        return $"t={timestamp},sign={Convert.ToHexStringLower(hash)}";
    }

    private string ComputeFood99Signature(string secret) {
        byte[] hash = MD5.HashData(
            Encoding.UTF8.GetBytes($"{_payload}{secret}")
        );
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
