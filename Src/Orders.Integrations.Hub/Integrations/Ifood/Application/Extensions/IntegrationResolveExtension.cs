using Orders.Integrations.Hub.Integrations.Ifood.Domain.ValueObjects;

namespace Orders.Integrations.Hub.Integrations.Ifood.Application.Extensions;

public static class IntegrationResolveExtension
{
    public static IfoodIntegrationResolved Resolve(this object integration)
    {
        // string merchantId = integration.Settings.FirstOrDefault(setting => setting.Name is "ifood_merchant_id")?.Value ?? throw new Exception("ifood merchant id");
        // string autoAcceptString = integration.Settings.FirstOrDefault(setting => setting.Name is "ifood_auto_accept")?.Value ?? "false";

        // bool autoAccept = bool.TryParse(autoAcceptString, out var result) && result;

        string merchantId = string.Empty;
        bool autoAccept = false;

        return new IfoodIntegrationResolved(
            IfoodMerchantId: merchantId,
            AutoAccept: autoAccept
        );
    }
}