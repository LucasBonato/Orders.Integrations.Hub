using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity.Dispute;

public record DisputeAlternative(
    string AlternativeId,
    AlternativeType Type,
    Price? Price,
    List<int>? AllowedTimesInMinutes,
    List<string>? AllowedTimesReasons
);