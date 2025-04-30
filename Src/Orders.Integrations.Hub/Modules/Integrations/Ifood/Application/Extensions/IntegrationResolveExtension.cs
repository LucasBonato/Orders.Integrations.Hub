using Orders.Integrations.Hub.Modules.Core..Domain.ValueObjects;
using Orders.Integrations.Hub.Modules.Integrations.Ifood.Domain.ValueObjects;

namespace Orders.Integrations.Hub.Modules.Integrations.Ifood.Application.Extensions;

public static class IntegrationResolveExtension
{
    public static IfoodIntegrationResolved Resolve(this IntegrationResponse integration)
    {
        string merchantId = integration.Settings.FirstOrDefault(setting => setting.Name is "ifood_merchant_id")?.Value ?? throw new Exception("ifood merchant id");
        string autoAcceptString = integration.Settings.FirstOrDefault(setting => setting.Name is "ifood_auto_accept")?.Value ?? "false";

        bool autoAccept = bool.TryParse(autoAcceptString, out var result) && result;

        return new IfoodIntegrationResolved(
            IfoodMerchantId: merchantId,
            AutoAccept: autoAccept
        );
    }
}