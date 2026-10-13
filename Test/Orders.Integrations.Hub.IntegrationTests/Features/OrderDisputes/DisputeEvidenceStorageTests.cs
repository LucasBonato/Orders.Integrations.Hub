using System.Net;

using Amazon.S3;
using Amazon.S3.Model;

using Orders.Integrations.Hub.IntegrationTests.Infrastructure;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Aws;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures.Collections;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Helpers;
using Orders.Integrations.Hub.IntegrationTests.Payloads;
using Orders.Integrations.Hub.TestCommon.Contracts.IFood;
using Orders.Integrations.Hub.TestCommon.Host;

namespace Orders.Integrations.Hub.IntegrationTests.Features.OrderDisputes;

[Collection(IntegrationTestCollection.Name)]
public sealed class DisputeEvidenceStorageTests(
    IntegrationHostFixture fixture
) : IntegrationTestBase(fixture) {
    [Fact]
    public async Task WebhookDispute_ShouldUploadEvidenceToS3_WhenDisputeContainsEvidence() {
        // Arrange
        Host.ConfigureIntegrationContext(IntegrationTestData.GetIntegrationContext());
        
        Host.WireMock.IFoodApi.StubEvidence();
        Host.WireMock.OrdersApi.StubPatchOrderDispute();
        string payload = PayloadLoader
            .Load("IFood", "dispute")
            .Replace(
                "https://media.test/evidence-1.png", 
                $"{Host.WireMock.IFood.Url}/evidence-1.png", 
                StringComparison.Ordinal
            );

        // Act
        using HttpResponseMessage result = await Host.Http.PostSignedAsync(
            IFoodContract.Instance.Webhook.SignatureRoute,
            payload,
            IFoodContract.Instance
        );

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
        await Host.WireMock.OrdersApi.WaitForPatchOrderDisputeAsync(TestContext.Current.CancellationToken);

        using AmazonS3Client s3 = LocalStackAwsClients.S3(Environment);
        ListObjectsV2Response listing = await s3.ListObjectsV2Async(
            new ListObjectsV2Request {
                BucketName = IntegrationTestEnvironment.S3BucketName,
                Prefix = "dispute/order-900/dispute-111/"
            }, 
            TestContext.Current.CancellationToken
        );

        S3Object evidence = Assert.Single(listing.S3Objects);
        GetObjectMetadataResponse metadata = await s3.GetObjectMetadataAsync(
            IntegrationTestEnvironment.S3BucketName,
            evidence.Key,
            TestContext.Current.CancellationToken
        );
        Assert.Equal("image/png", metadata.ContentType);
    }
}
