using Orders.Integrations.Hub.Core.Infrastructure.Integration;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Common.Validators;

namespace Orders.Integrations.Hub.Integrations.Food99;

[IntegrationKeyDefinition]
public static class Food99IntegrationKey {
    public const string Value = "FOOD99";
    public static IntegrationKey FOOD99 => IntegrationKey.From(Value);

    static Food99IntegrationKey() {
        IntegrationKeyValidator.ValidateRawValue(Value);
    }
}