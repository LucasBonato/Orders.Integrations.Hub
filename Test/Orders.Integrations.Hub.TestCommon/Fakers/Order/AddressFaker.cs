using Bogus;

using Orders.Integrations.Hub.Core.Domain.Entity.Address;

namespace Orders.Integrations.Hub.TestCommon.Fakers.Order;

public sealed class AddressFaker : Faker<Address> {
    public AddressFaker() {
        CustomInstantiator(faker => new Address(
            Country: "BR",
            State: faker.Address.StateAbbr(),
            City: faker.Address.City(),
            District: faker.Address.County(),
            Street: faker.Address.StreetName(),
            Number: faker.Random.Int(1, 9999).ToString(),
            Complement: faker.Random.Bool() ? faker.Address.SecondaryAddress() : string.Empty,
            Reference: faker.Lorem.Sentence(),
            FormattedAddress: faker.Address.FullAddress(),
            PostalCode: faker.Random.Replace("#####-###"),
            Coordinates: new AddressCoordinatesFaker().Generate()
        ));
    }
}