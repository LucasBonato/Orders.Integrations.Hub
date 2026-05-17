using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class PhoneFaker : Faker<Phone> {
    public PhoneFaker() {
        CustomInstantiator(faker => new Phone(
            Number: faker.Phone.PhoneNumber("###########"),
            Extension: faker.Random.Replace("##")
        ));
    }
}