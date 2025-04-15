using BizPik.Orders.Hub.Modules.Core.BizPik.Domain.ValueObjects;
using BizPik.Orders.Hub.Modules.Integrations.Ifood.Domain.ValueObjects;

namespace BizPik.Orders.Hub.Modules.Integrations.Ifood.Application.Extensions;

public static class IntegrationResolveExtension
{
    public static IfoodIntegrationResolved Resolve(this BizPikIntegrationResponse integration)
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