using System.Net;
using System.Text;

using Orders.Integrations.Hub.Core.Application.DTOs.Request;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures.Collections;
using Orders.Integrations.Hub.TestCommon.Host;
using Orders.Integrations.Hub.TestCommon.Mocks;

namespace Orders.Integrations.Hub.IntegrationTests.Features.OrderDisputes;

[Collection(IntegrationTestCollection.Name)]
public sealed class OrderDisputeEndpointTests(
    IntegrationHostFixture fixture
) : IntegrationTestBase(fixture) {
    private const string Route = "/api/v1/orders-hub/orders/disputes";

    [Fact]
    public async Task Post_ShouldReturnNoContent_AndCallIFood_WhenAcceptingDispute() {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext());
        
        await Host.WireMock.IFoodApi.StubTokenAsync(cancellationToken: TestContext.Current.CancellationToken);
        await Host.WireMock.IFoodApi.StubCommandEndpointsAsync(TestContext.Current.CancellationToken);

        // Act
        using HttpResponseMessage result = await SendAsync();

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        Assert.Equal(1, await Host.WireMock.IFoodApi.RequestCountAsync(
            "/order/v1.0/disputes/dispute-1/accept",
            TestContext.Current.CancellationToken
        ));
    }

    [Fact]
    public async Task Post_ShouldReturnProblem_WhenCounterOfferDoesNotHaveAnAlternative() {
        // Act
        using HttpResponseMessage result = await SendAsync(DisputeResponseType.COUNTER_OFFER);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("application/problem+json", result.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Post_ShouldReturnProblem_WhenIntegrationDoesNotSupportDisputes() {
        // Act
        using HttpResponseMessage result = await SendAsync(DisputeResponseType.ACCEPT, "RAPPI");

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, result.StatusCode);
    }

    private Task<HttpResponseMessage> SendAsync(DisputeResponseType type = DisputeResponseType.ACCEPT, string integration = "IFOOD") {
        string body = Serializer.Serialize(
            new RespondDisputeIntegrationRequest(
                DisputeId: "dispute-1",
                Integration: IntegrationKey.From(integration),
                Type: type,
                AlternativeId: null,
                DisputeResponse: new RespondDisputeResponse(
                    Reason: "Customer request",
                    DetailsReason: null,
                    Type: null,
                    Price: null,
                    AdditionalTimeInMinutes: null,
                    AdditionalTimeReason: null
                )
            )
        );

        return Host.Http.PostAsync(
            Route,
            new StringContent(body, Encoding.UTF8, "application/json"),
            TestContext.Current.CancellationToken
        );
    }
}
