using Orders.Integrations.Hub.Core.Application.DTOs.Internal;

namespace Orders.Integrations.Hub.Integrations.Common.Extensions;

public static class IntegrationResolverExtension
{
    public static IntegrationResolved ResolveIFood(this IntegrationResponse integration)
    {
        string merchantId = integration.GetSettingValue("ifood_merchant_id");
        string clientId = integration.GetClientIdValue("ifood");
        string clientSecret = integration.GetClientSecretValue("ifood");
        bool autoAccept = integration.GetAutoAcceptProperty();

        return new IntegrationResolved(
            TenantId: integration.TenantId.ToString(),
            MerchantId: merchantId,
            ClientId: clientId,
            ClientSecret: clientSecret,
            AutoAccept: autoAccept
        );
    }

    public static IntegrationResolved ResolveRappi(this IntegrationResponse integration)
    {
        string storeId = integration.GetSettingValue("rappi_store_id");
        string clientId = integration.GetClientIdValue("rappi");
        string clientSecret = integration.GetClientSecretValue("rappi");
        bool autoAccept = integration.GetAutoAcceptProperty();

        return new IntegrationResolved(
            TenantId: integration.TenantId.ToString(),
            MerchantId: storeId,
            ClientId: clientId,
            ClientSecret: clientSecret,
            AutoAccept: autoAccept
        );
    }

    public static IntegrationResolved Resolve99Food(this IntegrationResponse integration)
    {
        string appShopId = integration.GetSettingValue("99food_app_shop_id");
        string clientId = integration.GetClientIdValue("99food");
        string clientSecret = integration.GetClientSecretValue("99food");
        bool autoAccept = integration.GetAutoAcceptProperty();

        return new IntegrationResolved(
            TenantId: integration.TenantId.ToString(),
            MerchantId: appShopId,
            ClientId: clientId,
            ClientSecret: clientSecret,
            AutoAccept: autoAccept
        );
    }

    private static bool GetAutoAcceptProperty(this IntegrationResponse integration) {
        string autoAcceptString = integration.Settings.FirstOrDefault(setting => setting.Name is "enable_auto_accept")?.Value ?? "false";

        return bool.TryParse(autoAcceptString, out var result) && result;
    }

    private static string GetClientIdValue(this IntegrationResponse integration, string prefix) => GetSettingValue(integration, $"{prefix}_client_id");

    private static string GetClientSecretValue(this IntegrationResponse integration, string prefix) => GetSettingValue(integration, $"{prefix}_client_secret");

    private static string GetSettingValue(this IntegrationResponse integration, string settingName) {
        return integration.Settings.FirstOrDefault(setting => setting.Name == settingName)?.Value ?? throw new Exception($"{settingName} not found");
    }
}