namespace Orders.Integrations.Hub.Integrations.Common;

/// <summary>
/// Marker interface for integration key definition classes
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class IntegrationKeyDefinitionAttribute : Attribute { }