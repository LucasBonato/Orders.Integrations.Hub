using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Orders.Integrations.Hub.TestCommon.Mocks;

public sealed class WireMockAdminClient(string baseUrl) : IDisposable
{
    private readonly HttpClient _client = new() { BaseAddress = new Uri(baseUrl) };

    public async Task RegisterStubAsync(
        string path,
        string method,
        int statusCode,
        string body,
        string contentType,
        CancellationToken cancellationToken = default
    ) {
        string pathPattern = System.Text.RegularExpressions.Regex.Escape(path)
            .Replace("\\*", ".*", StringComparison.Ordinal);
        object mapping = new {
            request = new {
                method,
                urlPathPattern = pathPattern
            },
            response = new {
                status = statusCode,
                headers = new Dictionary<string, string> { ["Content-Type"] = contentType },
                body
            }
        };

        HttpResponseMessage response = await _client.PostAsJsonAsync("/__admin/mappings", mapping, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private async Task<List<WireMockRequest>> GetRequestsAsync(CancellationToken cancellationToken = default) {
        HttpResponseMessage response = await _client.GetAsync("/__admin/requests", cancellationToken);
        response.EnsureSuccessStatusCode();

        await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        WireMockRequestsResponse? result = await JsonSerializer.DeserializeAsync<WireMockRequestsResponse>(
            stream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
            cancellationToken
        );

        return result?.Requests?.Select(item => new WireMockRequest
        {
            Path = item.Request?.Url,
            Body = item.Request?.Body
        }).ToList() ?? [];
    }

    public async Task<int> RequestCountAsync(string path, CancellationToken cancellationToken = default)
        => (await GetRequestsAsync(cancellationToken))
            .Count(request => string.Equals(request.Path, path, StringComparison.OrdinalIgnoreCase));

    private async Task ResetRequestsAsync(CancellationToken cancellationToken = default) {
        HttpResponseMessage response = await _client.DeleteAsync("/__admin/requests", cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private async Task ResetMappingsAsync(CancellationToken cancellationToken = default) {
        HttpResponseMessage response = await _client.PostAsync("/__admin/mappings/reset", null, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task ResetAllAsync(CancellationToken cancellationToken = default) {
        await ResetRequestsAsync(cancellationToken);
        await ResetMappingsAsync(cancellationToken);
    }

    public async Task WaitForRequestAsync(
        string path,
        CancellationToken cancellationToken,
        TimeSpan? timeout = null
    ) {
        using CancellationTokenSource timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutSource.CancelAfter(timeout ?? TimeSpan.FromSeconds(10));

        TimeSpan pollInterval = TimeSpan.FromMilliseconds(100);

        while (!timeoutSource.Token.IsCancellationRequested)
        {
            List<WireMockRequest> requests = await GetRequestsAsync(timeoutSource.Token);
            if (requests.Any(r => string.Equals(r.Path, path, StringComparison.OrdinalIgnoreCase)))
                return;

            await Task.Delay(pollInterval, timeoutSource.Token);
        }

        List<WireMockRequest> finalRequests = await GetRequestsAsync(cancellationToken);
        string actualPaths = string.Join(", ", finalRequests.Select(r => r.Path));
        throw new TimeoutException($"Timed out waiting for '{path}'. Recorded request paths: {actualPaths}");
    }

    public async Task<string?> LastRequestBodyAsync(string path, CancellationToken cancellationToken = default) {
        List<WireMockRequest> requests = await GetRequestsAsync(cancellationToken);
        WireMockRequest? last = requests
            .LastOrDefault(r => string.Equals(r.Path, path, StringComparison.OrdinalIgnoreCase));

        return last?.Body;
    }

    public void Dispose() => _client.Dispose();

    private sealed class WireMockRequest
    {
        public string? Path { get; set; }
        public string? Body { get; set; }
    }

    public sealed class WireMockRequestsResponse
    {
        [JsonPropertyName("requests")]
        public List<WireMockJournalEntry>? Requests { get; init; }
    }

    public sealed class WireMockJournalEntry
    {
        [JsonPropertyName("request")]
        public WireMockJournalRequest? Request { get; init; }
    }

    public sealed class WireMockJournalRequest
    {
        [JsonPropertyName("url")]
        public string? Url { get; init; }

        [JsonPropertyName("body")]
        public string? Body { get; init; }
    }
}
