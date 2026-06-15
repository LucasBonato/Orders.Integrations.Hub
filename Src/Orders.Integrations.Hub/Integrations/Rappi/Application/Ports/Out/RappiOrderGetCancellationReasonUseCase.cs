using Orders.Integrations.Hub.Integrations.Common.Application;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.Integrations.Rappi.Application.Ports.Out;

public class RappiOrderGetCancellationReasonUseCase : EnumBasedCancellationReasonUseCase<RappiOrderCancelType>;