using System.Diagnostics.CodeAnalysis;

using Xunit.Sdk;

namespace Orders.Integrations.Hub.IntegrationTests.Contracts;

/// <summary>
/// Teaches xUnit v3 how to serialize/deserialize any <see cref="IIntegrationContract"/> for
/// Theory data rows, without the interface or its implementers needing to know serialization
/// exists. The wire form is just <see cref="IntegrationDescriptor.Key"/> — contracts are
/// re-resolved from <see cref="IntegrationContractRegistry"/> on deserialization, which also
/// means Test Explorer displays and re-runs individual rows by their integration key.
/// </summary>
public sealed class IntegrationContractSerializer : IXunitSerializer {
    public bool IsSerializable(
        Type type,
        object? value,
        [NotNullWhen(false)] out string? failureReason
    ) {
        failureReason = null;
        return value is IIntegrationContract;
    }

    public string Serialize(object value) 
        => (value as IIntegrationContract)!.Descriptor.Key;

    public object Deserialize(Type type, string serializedValue) 
        => IntegrationContractRegistry.GetByKey(serializedValue);
}