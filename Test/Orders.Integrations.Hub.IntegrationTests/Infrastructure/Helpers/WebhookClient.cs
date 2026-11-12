using System.Text;

using Orders.Integrations.Hub.TestCommon.Contracts;
using Orders.Integrations.Hub.TestCommon.Signing;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Helpers;

public static class WebhookClient
{
    extension(HttpClient client)
    {
        public async Task<HttpResponseMessage> PostSignedAsync(
            string route,
            string payload,
            IIntegrationContract contract
        ) => await client.PostSignedAsync(route, payload, contract.Webhook.Signer, contract.Webhook.ValidSecret);

        public async Task<HttpResponseMessage> PostSignedAsync(
            string route,
            string payload,
            IWebhookSigner signer,
            string secret
        ) {
            using StringContent content = new(payload, Encoding.UTF8, "application/json");
            content.Headers.TryAddWithoutValidation(signer.HeaderName, signer.Compute(payload, secret));
            return await client.PostAsync(route, content);
        }

        public async Task<HttpResponseMessage> PostWebhookAsync(
            string route,
            string payload
        ) {
            using StringContent content = new(payload, Encoding.UTF8, "application/json");
            return await client.PostAsync(route, content);
        }
    }
}
