using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Dispute;
using Orders.Integrations.Hub.Core.Domain.Enums;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class DisputeAlternativeFaker : Faker<DisputeAlternative> {
    public DisputeAlternativeFaker() {
        CustomInstantiator(faker => new DisputeAlternative(
            AlternativeId: faker.Random.Guid().ToString(),
            Type: faker.PickRandom<AlternativeType>(),
            Price: faker.Random.Bool() ? new PriceFaker().Generate() : null,
            AllowedTimesInMinutes: faker.Random.Bool() ? [10, 20, 30] : null,
            AllowedTimesReasons: faker.Random.Bool() ? [faker.Lorem.Word(), faker.Lorem.Word()] : null
        ));
    }
}