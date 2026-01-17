using Orders.Integrations.Hub.Core.Application.DTOs;

namespace Orders.Integrations.Hub.Core.Application.Commands;

public record UpdateOrderStatusCommand(
    OrderUpdate OrderUpdate,
    IntegrationKey SalesChannel
) : ICommand;