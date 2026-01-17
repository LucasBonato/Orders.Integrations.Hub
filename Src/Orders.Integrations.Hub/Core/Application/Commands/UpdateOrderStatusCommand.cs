using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;

namespace Orders.Integrations.Hub.Core.Application.Commands;

public record UpdateOrderStatusCommand(
    OrderUpdate OrderUpdate,
    IntegrationKey SalesChannel
) : ICommand;