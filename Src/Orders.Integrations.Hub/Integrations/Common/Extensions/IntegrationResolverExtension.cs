using Orders.Integrations.Hub.Core.Domain.ValueObjects.DTOs.Internal;
using Orders.Integrations.Hub.Integrations.Food99.Domain.ValueObjects.DTOs;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.DTOs;

namespace Orders.Integrations.Hub.Integrations.Common.Extensions;

public static class IntegrationResolverExtension
{
    public static IFoodIntegrationResolved ResolveIFood(this IntegrationResponse integration)
    {
        string merchantId = integration.GetSettingValue("ifood_merchant_id");
        bool autoAccept = integration.GetAutoAcceptProperty();

        return new IFoodIntegrationResolved(
            IfoodMerchantId: merchantId,
            AutoAccept: autoAccept
        );
    }

    public static RappiIntegrationResolved ResolveRappi(this IntegrationResponse integration)
    {
        string storeId = integration.GetSettingValue("rappi_store_id");
        bool autoAccept = integration.GetAutoAcceptProperty();

        return new RappiIntegrationResolved(
            RappiStoreId: storeId,
            AutoAccept: autoAccept
        );
    }

    public static Food99IntegrationResolved Resolve99Food(this IntegrationResponse integration)
    {
        string appShopId = integration.GetSettingValue("99food_app_shop_id");
        bool autoAccept = integration.GetAutoAcceptProperty();

        return new Food99IntegrationResolved(
            Food99AppShopId: appShopId,
            AutoAccept: autoAccept
        );
    }

    private static bool GetAutoAcceptProperty(this IntegrationResponse integration) {
        string autoAcceptString = integration.Settings.FirstOrDefault(setting => setting.Name is "enable_auto_accept")?.Value ?? "false";

        return bool.TryParse(autoAcceptString, out var result) && result;
    }

    private static string GetSettingValue(this IntegrationResponse integration, string settingName) {
        return integration.Settings.FirstOrDefault(setting => setting.Name == settingName)?.Value ?? throw new Exception($"{settingName} not found");
    }
}