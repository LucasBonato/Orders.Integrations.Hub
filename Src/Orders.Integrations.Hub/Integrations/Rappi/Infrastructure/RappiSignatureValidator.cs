using Orders.Integrations.Hub.Core.Application.DTOs.Internal;
using Orders.Integrations.Hub.Core.Application.Ports.Out.Serialization;
using Orders.Integrations.Hub.Integrations.Common.Contracts;
using Orders.Integrations.Hub.Integrations.Common.Extensions;
using Orders.Integrations.Hub.Integrations.Common.Validators;
using Orders.Integrations.Hub.Integrations.Common.ValueObjects;
using Orders.Integrations.Hub.Integrations.Rappi.Application.ValueObjects;

namespace Orders.Integrations.Hub.Integrations.Rappi.Infrastructure;

public class RappiSignatureValidator(
    [FromKeyedServices(RappiIntegrationKey.Value)] ICustomJsonSerializer serializer,
    HmacSha256SignatureValidator signatureValidator
) : IWebhookSignatureValidator {
    public string HeaderName => "Rappi-Signature";
    public string IntegrationKey => RappiIntegrationKey.RAPPI;

    public Integration ResolveIntegration(IntegrationResponse integration) 
        => integration.ResolveRappi();

    public string PrepareSignaturePayload(string signature, string body) {
        Dictionary<string, string> parts = ParseHeader(signature);
        string reserializedBody = serializer.Serialize(serializer.Deserialize<object>(body));
        return $"{parts["t"]}.{reserializedBody}";
    }
    
    public bool ValidateSignature(string signature, string payload, string secret)
        => signatureValidator.IsValid(ParseHeader(signature)["sign"], payload, secret);
    
    private static Dictionary<string, string> ParseHeader(string header)
        => header.Split(",")
            .Select(part => part.Split("="))
            .ToDictionary(item => item[0], item => item[1]);
}