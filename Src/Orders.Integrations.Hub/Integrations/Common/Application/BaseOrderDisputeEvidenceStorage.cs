using Orders.Integrations.Hub.Core.Application.Ports.Out.Clients;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;

namespace Orders.Integrations.Hub.Integrations.Common.Application;

public abstract class BaseOrderDisputeEvidenceStorage<TEvidence>(
    IObjectStorageClient objectStorageClient
) : IOrderDisputeEvidenceStorage<TEvidence> {
    
    private static string FolderPath(string orderId, string disputeId) 
        => $"dispute/{orderId}/{disputeId}";
    
    protected static string Filepath(string orderId, string disputeId, string fileName)
        => $"{FolderPath(orderId, disputeId)}/{Guid.CreateVersion7()}_{fileName}";
    
    public async Task<List<DisputeEvidence>> MigrateEvidencesToStorage(
        string orderId, 
        string disputeId, 
        IEnumerable<TEvidence> evidences
    ) {
        DisputeEvidence[] results = await Task.WhenAll(
            evidences.Select(evidence => UploadEvidence(orderId, disputeId, evidence))
        );
        
        return [..results];
    }

    public Task DeleteDisputeEvidence(string orderId, string disputeId) {
        return objectStorageClient.DeleteFolder(FolderPath(orderId, disputeId));
    }

    public abstract Task<DisputeEvidence> UploadEvidence(string orderId, string disputeId, TEvidence evidence);
}