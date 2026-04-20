using Orders.Integrations.Hub.Core.Domain.ValueObjects;
using Orders.Integrations.Hub.Integrations.Common.Attributes;
using Orders.Integrations.Hub.Integrations.Common.Validators;

namespace Orders.Integrations.Hub.Integrations.Food99.Application.ValueObjects;

[IntegrationKeyDefinition]
public static class Food99IntegrationKey {
    public const string Value = "FOOD99";
    public static IntegrationKey FOOD99 => IntegrationKey.From(Value);

    static Food99IntegrationKey() {
        IntegrationKeyValidator.ValidateRawValue(Value);
    }
}