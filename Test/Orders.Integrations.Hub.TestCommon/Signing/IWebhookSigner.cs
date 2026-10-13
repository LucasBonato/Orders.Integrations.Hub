namespace Orders.Integrations.Hub.TestCommon.Signing;

public interface IWebhookSigner
{
    string HeaderName { get; }

    string Compute(string body, string secret);
}
