namespace Orders.Integrations.Hub.IntegrationTests.Contracts.Signing;

public interface IWebhookSigner
{
    string HeaderName { get; }

    string Compute(string body, string secret);
}