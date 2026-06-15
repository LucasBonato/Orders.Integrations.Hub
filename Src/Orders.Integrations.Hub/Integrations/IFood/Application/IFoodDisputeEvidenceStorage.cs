using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Contracts;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Handshake;

namespace Orders.Integrations.Hub.Integrations.IFood.Application;

public class IFoodDisputeEvidenceStorage(
    IObjectStorageClient objectStorageClient,
    IIFoodClient ifoodClient
) : BaseOrderDisputeEvidenceStorage<Media>(objectStorageClient) {
    private readonly IObjectStorageClient _objectStorageClient = objectStorageClient;

    public override async Task<DisputeEvidence> UploadEvidence(
        string orderId,
        string disputeId,
        Media evidence
    ) {
        string imageId = evidence.Url.Split('/').Last();
        DownloadFile file = await ifoodClient.GetDisputeImage(evidence.Url);

        string keyPath = Filepath(orderId, disputeId, imageId);

        await using Stream fileStream = new MemoryStream(file.Bytes);
        string storedKey = await _objectStorageClient.UploadFile(fileStream, file.ContentType, keyPath);

        string temporaryUrl = _objectStorageClient.GetTemporaryUrl(storedKey);
        
        return new DisputeEvidence(
            Key: temporaryUrl,
            ContentType: file.ContentType
        );
    }
}