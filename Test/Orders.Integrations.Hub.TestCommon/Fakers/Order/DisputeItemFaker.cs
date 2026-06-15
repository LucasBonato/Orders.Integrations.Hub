using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class DisputeItemFaker : Faker<DisputeItem> {
    public DisputeItemFaker() {
        CustomInstantiator(faker => new DisputeItem(
            ExternalId: faker.Random.Guid().ToString(),
            ExternalUniqueId: faker.Random.Bool() ? faker.Random.Guid().ToString() : null,
            Sku: faker.Random.AlphaNumeric(10).ToUpper(),
            Index: faker.IndexFaker,
            Quantity: faker.Random.Int(1, 5),
            Price: new PriceFaker().Generate(),
            ReasonMessage: faker.Lorem.Sentence()
        ));
    }
}