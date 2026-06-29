using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Customer;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Delivery;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Item;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order.Payments;
using Orders.Integrations.Hub.Integrations.IFood.Domain.ValueObjects.Enums;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.ValueObjects.Enums;

namespace Orders.Integrations.Hub.IntegrationTests.Infrastructure;

public static class TestDataFactory
{
    public static IFoodOrder CreateMinimalIFoodOrder(string orderId = "order-456") {
        return new IFoodOrder(
            Id: orderId,
            DisplayId: "display-456",
            OrderType: IFoodOrderType.DELIVERY,
            FoodOrderTiming: IFoodOrderTiming.IMMEDIATE,
            SalesChannel: SalesChannel.IFOOD,
            Category: Category.FOOD,
            CreatedAt: DateTime.UtcNow,
            PreparationStartDateTime: DateTime.UtcNow,
            IsTest: false,
            ExtraInfo: null,
            Merchant: new Merchant(Id: "merchant-1", Name: "Test Merchant"),
            Customer: new Customer(
                Id: "customer-1",
                Name: "Test Customer",
                DocumentNumber: "12345678901",
                OrdersCountOnMerchant: 1,
                Phone: null,
                Segmentation: "REGULAR"
            ),
            Items: new List<Item> {
                new(
                    Index: 1,
                    Id: "item-1",
                    UniqueId: "unique-1",
                    ImageUrl: "https://example.com/img.jpg",
                    ExternalCode: "EXT-001",
                    Ean: "1234567890123",
                    Name: "Test Item",
                    Type: "PRODUCT",
                    Quantity: 2,
                    Unit: Unit.UN,
                    UnitPrice: 10.00m,
                    Price: 20.00m,
                    ScalePrices: new ScalePrices(DefaultPrice: 10.00m, Scales: []),
                    OptionsPrice: 0m,
                    TotalPrice: 20.00m,
                    Observations: "",
                    Options: null
                )
            },
            Benefits: null,
            AdditionalFees: null,
            Total: new Total(
                AdditionalFees: 0m,
                SubTotal: 20.00m,
                DeliveryFee: 5.00m,
                Benefits: 0m,
                OrderAmount: 25.00m
            ),
            Payments: new Payments(
                Prepaid: 25.00m,
                Pending: 0m,
                Methods: new List<PaymentsMethod> {
                    new(
                        Value: 25.00m,
                        Currency: "BRL",
                        Type: IFoodMethodType.ONLINE,
                        Method: Method.CREDIT,
                        Wallet: null,
                        Card: new Card(Brand: "VISA"),
                        Cash: null,
                        Transaction: null
                    )
                }
            ),
            Picking: null,
            Delivery: new Delivery(
                Mode: Mode.DEFAULT,
                Description: "Standard delivery",
                DeliveredBy: DeliveredBy.IFOOD,
                DeliveryDateTime: DateTime.UtcNow.AddHours(1),
                DeliveryAddress: new DeliveryAddress(
                    StreetName: "Rua Teste",
                    StreetNumber: "123",
                    FormattedAddress: "Rua Teste, 123",
                    Neighborhood: "Centro",
                    Complement: "",
                    Reference: "",
                    PostalCode: "12345-678",
                    City: "São Paulo",
                    State: "SP",
                    Country: "Brasil",
                    Coordinates: new Coordinates(Latitude: -23.5505m, Longitude: -46.6333m)
                ),
                Observations: "",
                PickupCode: ""
            ),
            Takeout: null,
            DineIn: null,
            Indoor: null,
            Schedule: null,
            AdditionalInfo: null
        );
    }

    public static RappiOrder CreateMinimalRappiOrder(string orderId = "rappi-order-123") {
        return new RappiOrder(
            OrderDetail: new RappiOrderDetails(
                Discounts: null,
                OrderId: orderId,
                CookingTime: 15,
                MinCookingTime: 10,
                MaxCookingTime: 20,
                CreatedAt: new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc),
                DeliveryMethod: RappiOrderDeliveryMethod.delivery,
                PaymentMethod: RappiOrderPaymentMethod.PIX,
                BillingInformation: null,
                DeliveryInformation: new RappiOrderDeliveryInformation(
                    City: "São Paulo",
                    CompleteAddress: "Av Paulista, 1000",
                    StreetShortcut: "Av Paulista",
                    FederalUnit: "SP",
                    StreetNumber: "1000",
                    Neighborhood: "Bela Vista",
                    Complement: "Apt 42",
                    PostalCode: "01310-100",
                    StreetName: "Avenida Paulista"
                ),
                Totals: new RappiOrderTotals(
                    TotalProducts: 50.00m,
                    TotalDiscounts: 5.00m,
                    TotalProductsWithDiscount: 45.00m,
                    TotalProductsWithoutDiscount: 50.00m,
                    TotalOtherDiscounts: 0.00m,
                    TotalOrder: 55.00m,
                    TotalToPay: 55.00m,
                    Charges: new RappiOrderTotalsCharges(Shipping: 10.00m, ServiceFee: 0.00m),
                    OtherTotals: new RappiOrderTotalsOtherTotals(
                        TotalRappiCredits: 0,
                        TotalRappiPay: 0,
                        Tip: 0
                    )
                ),
                Items: new List<RappiOrderItem> {
                    new(
                        Price: 50.00m,
                        Sku: "SKU-001",
                        Id: "item-1",
                        Name: "Pizza Calabresa",
                        Type: RappiOrderItemType.product,
                        Comments: "Sem cebola",
                        UnitPriceWithDiscount: 22.50,
                        UnitPriceWithoutDiscount: 25.00,
                        PercentageDiscount: 10.0,
                        Quantity: 2,
                        Subitems: new List<RappiOrderSubItem> {
                            new(
                                Sku: "SUB-SKU-001",
                                Id: "subitem-1",
                                Name: "Borda Recheada",
                                Type: RappiOrderItemType.topping,
                                Price: 5.00m,
                                PercentageDiscount: 0,
                                Quantity: 1
                            )
                        }
                    )
                },
                DeliveryDiscount: null
            ),
            Customer: new RappiOrderCustomer(
                FirstName: "João",
                LastName: "Silva",
                PhoneNumber: "11999999999",
                DocumentType: "CPF",
                DocumentNumber: "12345678901"
            ),
            Store: new RappiOrderStore(
                InternalId: "store-int-1",
                ExternalId: "store-ext-1",
                Name: "Pizzaria do João"
            )
        );
    }
}