using System.Net;
using System.Text;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;

namespace Orders.Integrations.Hub.IntegrationTests.Entrypoints.Http;

public sealed class OrdersHubProductStatusEndpointTests : IntegrationTestBase
{
    [Theory]
    [InlineData("enable")]
    [InlineData("disable")]
    public async Task Post_ShouldReturnProblem_WhenProductIntegrationKeyIsNotRegistered(string action)
    {
        // Arrange
        const string body = "{\"Message\":\"{\\\"sku\\\":\\\"sku-1\\\"}\"}";

        // Act
        using StringContent content = new(body, Encoding.UTF8, "application/json");
        using HttpResponseMessage result = await Host.Http.PostAsync(
            $"/api/v1/orders-hub/orders/products/{action}",
            content,
            TestContext.Current.CancellationToken
        );

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, result.StatusCode);
        Assert.Equal("application/problem+json", result.Content.Headers.ContentType?.MediaType);
    }
}
