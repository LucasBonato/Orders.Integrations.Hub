using Orders.Integrations.Hub.Core.Application.Integration;
using Orders.Integrations.Hub.Integrations.Common.Validators;

namespace Orders.Integrations.Hub.Integrations.IFood;

public static class IfoodIntegrationKey {
    public const string Value = "IFOOD";
    public static IntegrationKey IFOOD => IntegrationKey.From(Value);

    static IfoodIntegrationKey() {
        IntegrationKeyValidator.ValidateRawValue(Value);
    }
}