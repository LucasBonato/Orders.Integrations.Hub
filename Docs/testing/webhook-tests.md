# Webhook Tests

Webhooks are the primary entry point for external integrations. Tests must validate signature security, merchant resolution, and the full webhook-to-command flow.

## Structure of a Webhook Test

Webhook tests use `WebApplicationFactory` + `TestContainers.Redis` to exercise the full HTTP pipeline:

```csharp
[Collection("Redis")]
public class RappiWebhookTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IInternalClient _internalClient;

    public RappiWebhookTests(WebApplicationFactory<Program> factory)
    {
        _internalClient = Substitute.For<IInternalClient>();
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_internalClient);
            });
        }).CreateClient();
    }
}
```

## WebhookSignatureFilter Testing

The `WebhookSignatureFilter<TRequest, TValidator, TResolver>` is tested by exercising the full endpoint. The filter: reads body, extracts signature from headers, deserializes request, resolves merchant ID, fetches integration config from `IInternalClient`, validates signature against secret, passes context to handler.

## Test Scenarios Per Integration

### Valid Signature

```csharp
[Fact]
public async Task Post_ShouldReturnOk_WhenSignatureValid()
{
    string body = """{"order_id":"ord-1","event":"order.created"}""";
    string signature = ComputeHmacSha256(body, "test-secret");

    _internalClient.GetIntegrationByExternalId(Arg.Any<string>())
        .Returns(new IntegrationResponse { ClientSecret = "test-secret" });

    HttpRequestMessage request = new(HttpMethod.Post, "/webhooks/rappi/order");
    request.Headers.Add("X-Rappi-Signature", signature);
    request.Content = new StringContent(body, Encoding.UTF8, "application/json");

    HttpResponseMessage response = await _client.SendAsync(request);
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}
```

### Tampered Signature

```csharp
[Fact]
public async Task Post_ShouldReturnUnauthorized_WhenSignatureInvalid()
{
    string body = """{"order_id":"ord-1"}""";
    HttpRequestMessage request = new(HttpMethod.Post, "/webhooks/rappi/order");
    request.Headers.Add("X-Rappi-Signature", "invalid-signature");
    request.Content = new StringContent(body, Encoding.UTF8, "application/json");

    HttpResponseMessage response = await _client.SendAsync(request);
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
}
```

### Missing Signature

```csharp
[Fact]
public async Task Post_ShouldReturnUnauthorized_WhenSignatureHeaderMissing()
{
    string body = """{"order_id":"ord-1"}""";
    HttpResponseMessage response = await _client.PostAsync(
        "/webhooks/rappi/order",
        new StringContent(body, Encoding.UTF8, "application/json"));
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
}
```

## Testing Merchant Resolution

Test both paths: merchant-specific (`GetIntegrationByExternalId`) and app-level (`TryGetAppLevelIntegration`):

```csharp
[Fact]
public async Task Post_ShouldUseAppLevelIntegration_WhenMerchantNotFound()
{
    _internalClient.GetIntegrationByExternalId(Arg.Any<string>())
        .Returns((IntegrationResponse?)null);
    _internalClient.TryGetAppLevelIntegration(Arg.Any<string>())
        .Returns(new IntegrationResponse { ClientSecret = "app-secret" });
}
```

## Full Webhook-to-Command Flow

Configure `WebApplicationFactory` with MassTransit harness and verify that a valid request results in a command being published:

```csharp
[Fact]
public async Task Post_ShouldPublishCommand_WhenSignatureValid()
{
    // Arrange: valid signature, mock internal client
    // Act: POST to webhook endpoint
    // Assert: ITestHarness.Published.Any<TCommand>() is true
}
```

## Per-Integration Payload Examples

Place fixtures in a `Fixtures/` directory under each integration's test folder:
- `rappi-webhook-create-order.json`
- `rappi-webhook-cancel-order.json`
- `ifood-webhook-status-change.json`
- `food99-webhook-order.json`
