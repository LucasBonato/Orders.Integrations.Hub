using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.Core.Domain.Entity;

public record OrderFee(
    string Name,
    FeeType Type,
    FeeReceivedBy ReceivedBy,
    Price Price,
    string ReceiverDocument,
    string Observation
);