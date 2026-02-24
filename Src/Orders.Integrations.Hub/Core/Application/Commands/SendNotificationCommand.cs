using Orders.Integrations.Hub.Core.Application.DTOs;

namespace Orders.Integrations.Hub.Core.Application.Commands;

public record SendNotificationCommand(
    OrderUpdate Message,
    string? TopicArn
) : ICommand;