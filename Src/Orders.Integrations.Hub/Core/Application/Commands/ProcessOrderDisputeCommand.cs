using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.Core.Application.Commands;

public record ProcessOrderDisputeCommand(
    string ExternalOrderId,
    IntegrationKey Integration,
    OrderDispute? OrderDispute,
    OrderEventType Type
) : ICommand;