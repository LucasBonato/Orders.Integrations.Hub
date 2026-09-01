using System.Net;
using System.Text;
using System.Text.Json;

using Orders.Integrations.Hub.IntegrationTests.Contracts;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Extensions;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Mocks;

namespace Orders.Integrations.Hub.IntegrationTests.Entrypoints.Http;

public sealed class OrdersHubOrderStatusEndpointTests : IntegrationTestBase
{
    private const string Route = "/api/v1/orders-hub/orders/status";

    public static TheoryData<IIntegrationContract> Subjects => new(IntegrationContractRegistry.All);

    [Theory]
    [MemberData(nameof(Subjects))]
    public async Task Patch_ShouldReturnNoContent_WhenConfirmingOrder_ForEveryIntegration(IIntegrationContract contract)
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext());
        Host.WireMock.StubIntegration(contract);

        string merchantId = $"{contract.Descriptor.Key.ToLowerInvariant()}-merchant-id";
        string body = JsonSerializer.Serialize(new {
            orderId = "ord-1",
            externalId = "ext-1",
            merchantId,
            status = "CONFIRMED",
            integration = contract.Descriptor.IntegrationQueryValue
        });

        // Act
        using HttpResponseMessage result = await Host.Http.PatchAsync(
            Route,
            new StringContent(body, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken
        );

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
    }

    [Fact]
    public async Task Patch_ShouldCallIFoodConfirmEndpoint()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext());
        
        Host.WireMock.IFoodApi.StubToken();
        Host.WireMock.IFoodApi.StubCommandEndpoints();

        // Act
        using HttpResponseMessage result = await SendStatusAsync("IFOOD", "ifood-merchant");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        Assert.Equal(1, WireMockApi.RequestCount(Host.WireMock.IFood, "/order/v1.0/orders/ext-1/confirm"));
    }

    [Fact]
    public async Task Patch_ShouldReturnProblem_WhenIntegrationIsUnknown()
    {
        // Act
        using HttpResponseMessage result = await SendStatusAsync("UNKNOWN", "ifood-merchant");

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, result.StatusCode);
        Assert.Equal("application/problem+json", result.Content.Headers.ContentType?.MediaType);
    }

    private Task<HttpResponseMessage> SendStatusAsync(string integration, string merchantId) {
        string body = JsonSerializer.Serialize(new {
            orderId = "ord-1",
            externalId = "ext-1",
            merchantId,
            status = "CONFIRMED",
            integration
        });

        return Host.Http.PatchAsync(
            Route,
            new StringContent(body, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken
        );
    }
}
