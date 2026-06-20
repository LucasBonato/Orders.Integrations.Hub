using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;
using Orders.Integrations.Hub.Integrations.IFood.Application.Clients;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;

namespace Orders.Integrations.Hub.Integrations.IFood.Infrastructure;

public class IFoodDisputeEvidenceStorage(
    IObjectStorageClient objectStorageClient,
    IHttpClientFactory httpClientFactory
) : BaseOrderDisputeEvidenceStorage<Media>(objectStorageClient) {
    private readonly IObjectStorageClient _objectStorageClient = objectStorageClient;

    protected override async Task<DisputeEvidence> UploadEvidence(
        string orderId,
        string disputeId,
        Media evidence
    ) {
        string imageId = evidence.Url.Split('/').Last();

        using HttpClient client = httpClientFactory.CreateClient(typeof(IIFoodClient).FullName!);
        HttpRequestMessage request = new(HttpMethod.Get, evidence.Url);
        HttpResponseMessage response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException(await response.Content.ReadAsStringAsync());

        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
        string contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";

        string keyPath = Filepath(orderId, disputeId, imageId);
        await using Stream fileStream = new MemoryStream(fileBytes);
        string storedKey = await _objectStorageClient.UploadFile(fileStream, contentType, keyPath);
        string temporaryUrl = _objectStorageClient.GetTemporaryUrl(storedKey);
        
        return new DisputeEvidence(
            Key: temporaryUrl,
            ContentType: contentType
        );
    }
}
