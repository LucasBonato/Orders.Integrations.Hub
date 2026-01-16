using Orders.Integrations.Hub.Core.Infrastructure.Integration;
using Orders.Integrations.Hub.Integrations.Common;
using Orders.Integrations.Hub.Integrations.Common.Validators;

namespace Orders.Integrations.Hub.Integrations.Rappi;

[IntegrationKeyDefinition]
public static class RappiIntegrationKey {
    public const string Value = "RAPPI";
    public static readonly IntegrationKey RAPPI = IntegrationKey.From(Value);

    static RappiIntegrationKey() {
        IntegrationKeyValidator.ValidateRawValue(Value);
    }
}