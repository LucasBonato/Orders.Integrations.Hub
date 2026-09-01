using System.Net;
using System.Text;
using System.Text.Json;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Mocks;

namespace Orders.Integrations.Hub.IntegrationTests.Entrypoints.Http;

public sealed class OrdersHubOrderDisputeEndpointTests : IntegrationTestBase
{
    private const string Route = "/api/v1/orders-hub/orders/disputes";

    [Fact]
    public async Task Post_ShouldReturnNoContent_AndCallIFood_WhenAcceptingDispute() {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext());
        
        Host.WireMock.IFoodApi.StubToken();
        Host.WireMock.IFoodApi.StubCommandEndpoints();

        // Act
        using HttpResponseMessage result = await SendAsync("ACCEPT");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        Assert.Equal(1, WireMockApi.RequestCount(Host.WireMock.IFood, "/order/v1.0/disputes/dispute-1/accept"));
    }

    [Fact]
    public async Task Post_ShouldReturnProblem_WhenCounterOfferDoesNotHaveAnAlternative() {
        // Act
        using HttpResponseMessage result = await SendAsync("COUNTER_OFFER");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("application/problem+json", result.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Post_ShouldReturnProblem_WhenIntegrationDoesNotSupportDisputes() {
        // Act
        using HttpResponseMessage result = await SendAsync("ACCEPT", "RAPPI");

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, result.StatusCode);
    }

    private Task<HttpResponseMessage> SendAsync(string type, string integration = "IFOOD") {
        string body = JsonSerializer.Serialize(
            new {
                disputeId = "dispute-1",
                integration,
                type,
                alternativeId = (string?)null,
                disputeResponse = new {
                    reason = "Customer request",
                    detailsReason = (string?)null,
                    type = (string?)null,
                    price = (object?)null,
                    additionalTimeInMinutes = (int?)null,
                    additionalTimeReason = (string?)null
                }
            }
        );

        return Host.Http.PostAsync(
            Route,
            new StringContent(body, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken
        );
    }
}
