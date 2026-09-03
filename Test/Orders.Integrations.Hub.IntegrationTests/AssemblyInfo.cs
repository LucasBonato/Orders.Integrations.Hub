using Xunit.Sdk;

using Orders.Integrations.Hub.IntegrationTests.Contracts;

[assembly: RegisterXunitSerializer(typeof(IntegrationContractSerializer), typeof(IIntegrationContract))]