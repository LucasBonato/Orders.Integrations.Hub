using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;

namespace Orders.Integrations.Hub.Integrations.Common.Extensions;

public static class IntegrationResolverExtension
{
    extension(IntegrationResponse integration)
    {
        public Integration ResolveIFood()
        {
            const string prefix = "ifood";
            string merchantId = integration.GetSettingValue($"{prefix}_merchant_id");
            string clientId = integration.GetClientIdValue(prefix);
            string clientSecret = integration.GetClientSecretValue(prefix);
            bool autoAccept = integration.GetAutoAcceptProperty();
            IntegrationMode mode = integration.GetModeValue(prefix);

            return new Integration(
                TenantId: integration.TenantId.ToString(),
                MerchantId: merchantId,
                ClientId: clientId,
                ClientSecret: clientSecret,
                AutoAccept: autoAccept,
                Mode: mode
            );
        }

        public Integration ResolveRappi()
        {
            const string prefix = "rappi";
            string storeId = integration.GetSettingValue($"{prefix}_store_id");
            string clientId = integration.GetClientIdValue(prefix);
            string clientSecret = integration.GetClientSecretValue(prefix);
            bool autoAccept = integration.GetAutoAcceptProperty();
            IntegrationMode mode = integration.GetModeValue(prefix);

            return new Integration(
                TenantId: integration.TenantId.ToString(),
                MerchantId: storeId,
                ClientId: clientId,
                ClientSecret: clientSecret,
                AutoAccept: autoAccept,
                Mode: mode
            );
        }

        public Integration Resolve99Food()
        {
            const string prefix = "99food";
            string appShopId = integration.GetSettingValue($"{prefix}_app_shop_id");
            string clientId = integration.GetClientIdValue(prefix);
            string clientSecret = integration.GetClientSecretValue(prefix);
            bool autoAccept = integration.GetAutoAcceptProperty();
            IntegrationMode mode = integration.GetModeValue(prefix);

            return new Integration(
                TenantId: integration.TenantId.ToString(),
                MerchantId: appShopId,
                ClientId: clientId,
                ClientSecret: clientSecret,
                AutoAccept: autoAccept,
                Mode: mode
            );
        }

        private bool GetAutoAcceptProperty() {
            string autoAcceptString = integration.Settings.FirstOrDefault(setting => setting.Name is "enable_auto_accept")?.Value ?? "false";

            return bool.TryParse(autoAcceptString, out var result) && result;
        }

        private IntegrationMode GetModeValue(string prefix)
            => integration.GetSettingValue($"{prefix}_mode") == "Centralized" 
                ? IntegrationMode.Centralized
                : IntegrationMode.Distributed;

        private string GetClientIdValue(string prefix) 
            => integration.GetSettingValue($"{prefix}_client_id");

        private string GetClientSecretValue(string prefix) 
            => integration.GetSettingValue($"{prefix}_client_secret");

        private string GetSettingValue(string settingName)
            => integration.Settings.FirstOrDefault(setting => setting.Name == settingName)?.Value 
               ?? throw new Exception($"{settingName} not found");
    }
}