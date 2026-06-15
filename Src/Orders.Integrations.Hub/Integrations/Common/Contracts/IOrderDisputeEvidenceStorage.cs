using Orders.Integrations.Hub.Integrations.Common.ValueObjects;

namespace Orders.Integrations.Hub.Integrations.Common.Contracts;

public interface IOrderDisputeEvidenceStorage<in TEvidence> {
    Task<List<DisputeEvidence>> MigrateEvidencesToStorage(string orderId, string disputeId, IEnumerable<TEvidence> evidences);
    Task DeleteDisputeEvidence(string orderId, string disputeId);
}