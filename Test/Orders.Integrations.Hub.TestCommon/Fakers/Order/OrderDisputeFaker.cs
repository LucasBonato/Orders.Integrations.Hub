using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class OrderDisputeFaker : Faker<OrderDispute> {
    public OrderDisputeFaker() {
        CustomInstantiator(faker => new OrderDispute(
            DisputeId: faker.Random.Guid().ToString(),
            Message: faker.Lorem.Sentence(),
            Action: faker.PickRandom("ACCEPT", "REJECT", "PARTIAL"),
            TimeoutAction: faker.PickRandom("ACCEPT", "REJECT"),
            CreatedAt: faker.Date.Recent(),
            ExpiresAt: faker.Date.Soon(),
            Alternatives: new DisputeAlternativeFaker().Generate(faker.Random.Int(1, 3)),
            Evidences: faker.Random.Bool() ? [new DisputeEvidence(faker.Internet.Url())] : null,
            Items: new DisputeItemFaker().Generate(faker.Random.Int(1, 3)),
            Options: new DisputeItemOptionFaker().Generate(faker.Random.Int(0, 2)),
            CancellationReasons: faker.Random.Bool() ? [faker.Lorem.Word(), faker.Lorem.Word()] : null
        ));
    }
}