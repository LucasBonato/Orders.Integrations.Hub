using Mono.Cecil;

using NetArchTest.Rules;

namespace Orders.Integrations.Hub.ArchTests;

public class EndsWithRequestOrResponse : ICustomRule
{
    public bool MeetsRule(TypeDefinition type)
    {
        return type.Name.EndsWith("Request") || type.Name.EndsWith("Response");
    }
}