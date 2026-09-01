using System.Net;
using System.Text.Json;

using Orders.Integrations.Hub.Core.Application.DTOs.Response;
using Orders.Integrations.Hub.IntegrationTests.Contracts;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Extensions;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Mocks;

namespace Orders.Integrations.Hub.IntegrationTests.Entrypoints.Http;

public sealed class OrdersHubCancellationReasonsEndpointTests : IntegrationTestBase {
    private const string Route = "/api/v1/orders-hub/orders/cancellation-reasons";

    public static TheoryData<IIntegrationContract> Subjects => new(IntegrationContractRegistry.All);

    [Theory]
    [MemberData(nameof(Subjects))]
    public async Task Get_ShouldReturnCancellationReasons_ForEveryIntegration(IIntegrationContract contract)
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(contract));

        if (contract.Descriptor.Key == "IFood") {
            Host.WireMock.IFoodApi.StubCancellationReasons();
            Host.WireMock.StubIntegration(contract);
        }

        // Act
        using HttpResponseMessage result = await Host.Http.GetAsync(
            $"{Route}?integration={contract.Descriptor.IntegrationQueryValue}&externalOrderId=ext-1",
            TestContext.Current.CancellationToken
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        List<CancellationReasonsResponse>? reasons = JsonSerializer.Deserialize<List<CancellationReasonsResponse>>(
            await result.Content.ReadAsStringAsync(TestContext.Current.CancellationToken),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        Assert.NotNull(reasons);
        Assert.NotEmpty(reasons);
    }

    [Fact]
    public async Task Get_ShouldCallIFoodCancellationReasonsEndpoint()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext());
        
        Host.WireMock.IFoodApi.StubToken();
        Host.WireMock.IFoodApi.StubCancellationReasons();

        // Act
        using HttpResponseMessage result = await Host.Http.GetAsync(
            $"{Route}?integration=IFOOD&externalOrderId=ext-1",
            TestContext.Current.CancellationToken
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(1, WireMockApi.RequestCount(Host.WireMock.IFood, "/order/v1.0/orders/ext-1/cancellationReasons"));
    }

    [Fact]
    public async Task Get_ShouldReturnProblem_WhenIntegrationIsUnknown()
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext());
        
        // Act
        using HttpResponseMessage result = await Host.Http.GetAsync(
            $"{Route}?integration=UNKNOWN&externalOrderId=ext-1",
            TestContext.Current.CancellationToken
        );

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, result.StatusCode);
        Assert.Equal("application/problem+json", result.Content.Headers.ContentType?.MediaType);
    }
}
