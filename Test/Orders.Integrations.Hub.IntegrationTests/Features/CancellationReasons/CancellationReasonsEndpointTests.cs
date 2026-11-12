using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

using Orders.Integrations.Hub.Core.Application.DTOs.Response;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures.Collections;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Helpers;
using Orders.Integrations.Hub.TestCommon.Contracts;
using Orders.Integrations.Hub.TestCommon.Host;

namespace Orders.Integrations.Hub.IntegrationTests.Features.CancellationReasons;

[Collection(IntegrationTestCollection.Name)]
public sealed class CancellationReasonsEndpointTests(
    IntegrationHostFixture fixture
) : IntegrationTestBase(fixture) {
    private const string Route = "/api/v1/orders-hub/orders/cancellation-reasons";

    public static TheoryData<IIntegrationContract> Subjects => new(IntegrationContractRegistry.All);

    [Theory]
    [MemberData(nameof(Subjects))]
    public async Task Get_ShouldReturnCancellationReasons_ForEveryIntegration(IIntegrationContract contract)
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(contract));

        if (contract.Descriptor.Key == "IFood") {
            await Host.WireMock.IFoodApi.StubCancellationReasonsAsync(TestContext.Current.CancellationToken);
            await Host.WireMock.StubIntegrationAsync(contract, TestContext.Current.CancellationToken);
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
        
        await Host.WireMock.IFoodApi.StubTokenAsync(cancellationToken: TestContext.Current.CancellationToken);
        await Host.WireMock.IFoodApi.StubCancellationReasonsAsync(TestContext.Current.CancellationToken);

        // Act
        using HttpRequestMessage request = new(HttpMethod.Get, $"{Route}?integration=IFOOD&externalOrderId=ext-1");
        request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
        using HttpResponseMessage result = await Host.Http.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(1, await Host.WireMock.IFoodApi.RequestCountAsync(
            "/order/v1.0/orders/ext-1/cancellationReasons",
            TestContext.Current.CancellationToken
        ));
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
