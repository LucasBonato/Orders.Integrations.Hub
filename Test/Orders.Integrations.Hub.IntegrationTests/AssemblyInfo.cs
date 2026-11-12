using Orders.Integrations.Hub.TestCommon.Contracts;

using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(IntegrationContractSerializer), typeof(IIntegrationContract))]