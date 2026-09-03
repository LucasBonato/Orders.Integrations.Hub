namespace Orders.Integrations.Hub.IntegrationTests.Contracts;

public static class IntegrationContractRegistry
{
    public static IReadOnlyList<IIntegrationContract> All { get; } = Discover();
    
    private static readonly Dictionary<string, IIntegrationContract> ByKey = All.ToDictionary(contract => contract.Descriptor.Key, contract => contract);
    
    public static IIntegrationContract GetByKey(string key) => ByKey[key];
    
    private static IReadOnlyList<IIntegrationContract> Discover() => [..
            typeof(IntegrationContractRegistry).Assembly.GetTypes()
                .Where(type => type is { IsAbstract: false, IsClass: true } && type.IsAssignableTo(typeof(IIntegrationContract)))
                .Select(type => (IIntegrationContract)Activator.CreateInstance(type, nonPublic: true)!)
                .OrderBy(contract => contract.Descriptor.Key)
        ];
}