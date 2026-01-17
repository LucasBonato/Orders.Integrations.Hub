using Orders.Integrations.Hub.Core.Application.DTOs;
using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Common.Validators;

namespace Orders.Integrations.Hub.Integrations.IFood;

[IntegrationKeyDefinition]
public static class IFoodIntegrationKey {
    public const string Value = "IFOOD";
    public static IntegrationKey IFOOD => IntegrationKey.From(Value);

    static IFoodIntegrationKey() {
        IntegrationKeyValidator.ValidateRawValue(Value);
    }
}