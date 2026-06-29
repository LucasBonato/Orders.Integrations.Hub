using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.IFood.Application.Extensions;
using Orders.Integrations.Hub.Integrations.IFood.Application.ValueObjects;
using Orders.Integrations.Hub.Integrations.IFood.Domain.Entity.Order;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure;

namespace Orders.Integrations.Hub.IntegrationTests.Webhooks.IFood;

public sealed class IFoodOrderMappingTests
{
    [Fact]
    public void ToOrder_Should_Map_OrderId_Correctly()
    {
        IFoodOrder source = TestDataFactory.CreateMinimalIFoodOrder(orderId: "order-test-123");

        Order result = source.ToOrder("tenant-1");

        Assert.Equal("order-test-123", result.OrderId);
    }

    [Fact]
    public void ToOrder_Should_Map_Merchant_Correctly()
    {
        IFoodOrder source = TestDataFactory.CreateMinimalIFoodOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.Equal("merchant-1", result.VirtualBrand);
        Assert.Equal("merchant-1", result.Merchant.Id);
        Assert.Equal("Test Merchant", result.Merchant.Name);
    }

    [Fact]
    public void ToOrder_Should_Map_Customer_Correctly()
    {
        IFoodOrder source = TestDataFactory.CreateMinimalIFoodOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.Equal("customer-1", result.Customer?.Id);
        Assert.Equal("Test Customer", result.Customer?.Name);
        Assert.Equal("12345678901", result.Customer?.DocumentNumber);
    }

    [Fact]
    public void ToOrder_Should_Map_Items_Correctly()
    {
        IFoodOrder source = TestDataFactory.CreateMinimalIFoodOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.NotEmpty(result.Items);
        Assert.Equal("item-1", result.Items[0].Id);
        Assert.Equal("Test Item", result.Items[0].Name);
        Assert.Equal(2, result.Items[0].Quantity);
        Assert.Equal("EXT-001", result.Items[0].ExternalCode);
    }

    [Fact]
    public void ToOrder_Should_Map_Delivery_Correctly()
    {
        IFoodOrder source = TestDataFactory.CreateMinimalIFoodOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.NotNull(result.Delivery);
        Assert.Equal("Rua Teste, 123", result.Delivery!.DeliveryAddress?.FormattedAddress);
        Assert.Equal("São Paulo", result.Delivery!.DeliveryAddress?.City);
        Assert.Equal("SP", result.Delivery!.DeliveryAddress?.State);
    }

    [Fact]
    public void ToOrder_Should_Map_TenantId_Correctly()
    {
        IFoodOrder source = TestDataFactory.CreateMinimalIFoodOrder();

        Order result = source.ToOrder("custom-tenant");

        Assert.Equal("custom-tenant", result.TenantId);
    }

    [Fact]
    public void ToOrder_Should_Set_SourceAppId_To_IFOOD()
    {
        IFoodOrder source = TestDataFactory.CreateMinimalIFoodOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.Equal(IFoodIntegrationKey.IFOOD, result.SourceAppId);
    }

    [Fact]
    public void ToOrder_Should_Map_OrderTiming_To_Instant_For_Immediate()
    {
        IFoodOrder source = TestDataFactory.CreateMinimalIFoodOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.Equal(OrderTiming.INSTANT, result.OrderTiming);
    }

    [Fact]
    public void ToOrder_Should_Map_OrderType_Delivery_Correctly()
    {
        IFoodOrder source = TestDataFactory.CreateMinimalIFoodOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.Equal(OrderType.DELIVERY, result.Type);
    }
}