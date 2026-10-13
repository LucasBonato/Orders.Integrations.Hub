using System.Net;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures.Collections;
using Orders.Integrations.Hub.IntegrationTests.Payloads;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Helpers;
using Orders.Integrations.Hub.TestCommon.Contracts;
using Orders.Integrations.Hub.TestCommon.Host;

namespace Orders.Integrations.Hub.IntegrationTests.Features.WebhookSignature;

[Collection(IntegrationTestCollection.Name)]
public sealed class WebhookSignatureTests(
    IntegrationHostFixture fixture
) : IntegrationTestBase(fixture) {
    public static TheoryData<IIntegrationContract> Subjects => new(IntegrationContractRegistry.All);

    [Theory]
    [MemberData(nameof(Subjects))]
    public async Task Post_ShouldReturnUnauthorized_WhenSignatureHeaderIsMissing(IIntegrationContract contract)
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(contract));
        
        // Act
        using HttpResponseMessage result = await Host.Http.PostWebhookAsync(
            contract.Webhook.SignatureRoute,
            PayloadLoader.Load(
                contract.Descriptor.Key, 
                contract.Payloads.SignaturePayload
            )
        );

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(Subjects))]
    public async Task Post_ShouldReturnUnauthorized_WhenSignatureIsInvalid(IIntegrationContract contract)
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(contract));
        
        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            contract.Webhook.SignatureRoute,
            PayloadLoader.Load(
                contract.Descriptor.Key, 
                contract.Payloads.SignaturePayload
            ),
            contract.Webhook.Signer,
            "wrong-secret"
        );

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(Subjects))]
    public async Task Post_ShouldAccept_WhenSignatureIsValid(IIntegrationContract contract)
    {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext(contract));
        if (contract.Descriptor.Key is "Rappi" or "Food99")
            Host.WireMock.OrdersApi.StubCreateOrder();

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            contract.Webhook.SignatureRoute,
            PayloadLoader.Load(
                contract.Descriptor.Key, 
                contract.Payloads.SignaturePayload
            ),
            contract
        );

        // Assert
        Assert.InRange((int)result.StatusCode, 200, 299);

        if (contract.Descriptor.Key is "Rappi" or "Food99")
            await Host.WireMock.OrdersApi.WaitForCreateOrderAsync(TestContext.Current.CancellationToken);
    }
}
