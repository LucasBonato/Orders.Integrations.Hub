namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure.Fixtures.Collections;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationHostFixture>
{
    public const string Name = "IntegrationTestInfrastructure";
}
