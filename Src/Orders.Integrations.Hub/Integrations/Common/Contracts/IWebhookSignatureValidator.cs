using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;

namespace Orders.Integrations.Hub.Integrations.Common.Contracts;

public interface IWebhookSignatureValidator {
    string HeaderName { get; }
    string IntegrationKey { get; }

    bool ValidateSignature(string signature, string body, string secret);
    
    Integration ResolveIntegration(IntegrationResponse integration);
    
    string PrepareSignaturePayload(string signature, string body) => body;
}