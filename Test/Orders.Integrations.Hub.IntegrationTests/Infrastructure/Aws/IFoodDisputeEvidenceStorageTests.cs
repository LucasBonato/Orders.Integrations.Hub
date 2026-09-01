using System.Net;

using Amazon.S3;
using Amazon.S3.Model;

using Orders.Integrations.Hub.IntegrationTests.Contracts.IFood;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure.Host;
using Orders.Integrations.Hub.IntegrationTests.Payloads;
using Orders.Integrations.Hub.IntegrationTests.Requests;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Aws;

[Collection(IntegrationTestCollection.Name)]
public sealed class IFoodDisputeEvidenceStorageTests(
    TestInfrastructure infrastructure
) : IntegrationTestBase(infrastructure) {
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

        using AmazonS3Client s3 = LocalStackAwsClients.S3(Infrastructure!.Environment);
        ListObjectsV2Response listing = await s3.ListObjectsV2Async(
            new ListObjectsV2Request {
                BucketName = Infrastructure.Environment.S3BucketName,
                Prefix = "dispute/order-900/dispute-111/"
            }, 
            TestContext.Current.CancellationToken
        );

        S3Object evidence = Assert.Single(listing.S3Objects);
        GetObjectMetadataResponse metadata = await s3.GetObjectMetadataAsync(
            Infrastructure.Environment.S3BucketName,
            evidence.Key,
            TestContext.Current.CancellationToken
        );
        Assert.Equal("image/png", metadata.ContentType);
    }
}
