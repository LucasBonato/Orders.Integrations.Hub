using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.Ports.Out;

public class Food99OrderGetCancellationReasonUseCase : EnumBasedCancellationReasonUseCase<Food99OrderCancelType>;