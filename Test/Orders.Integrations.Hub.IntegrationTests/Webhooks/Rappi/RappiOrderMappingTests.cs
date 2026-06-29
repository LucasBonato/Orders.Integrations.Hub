using Orders.Integrations.Hub.Core.Domain.Entity;
using Orders.Integrations.Hub.Core.Domain.Enums;
using Orders.Integrations.Hub.Integrations.Rappi.Application.Extensions;
using Orders.Integrations.Hub.Integrations.Rappi.Application.ValueObjects;
using Orders.Integrations.Hub.Integrations.Rappi.Domain.Entity;
using Orders.Integrations.Hub.IntegrationTests.Infrastructure;

namespace Orders.Integrations.Hub.IntegrationTests.Webhooks.Rappi;

public sealed class RappiOrderMappingTests
{
    [Fact]
    public void ToOrder_Should_Map_OrderId_Correctly()
    {
        RappiOrder source = TestDataFactory.CreateMinimalRappiOrder(orderId: "rappi-order-999");

        Order result = source.ToOrder("tenant-1");

        Assert.Equal("rappi-order-999", result.OrderId);
    }

    [Fact]
    public void ToOrder_Should_Map_Store_Correctly()
    {
        RappiOrder source = TestDataFactory.CreateMinimalRappiOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.Equal("store-int-1", result.VirtualBrand);
        Assert.Equal("store-ext-1", result.Merchant.Id);
        Assert.Equal("Pizzaria do João", result.Merchant.Name);
    }

    [Fact]
    public void ToOrder_Should_Map_Customer_Correctly()
    {
        RappiOrder source = TestDataFactory.CreateMinimalRappiOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.Equal("João Silva", result.Customer?.Name);
        Assert.Equal("12345678901", result.Customer?.DocumentNumber);
        Assert.Equal("11999999999", result.Customer?.Phone?.Number);
    }

    [Fact]
    public void ToOrder_Should_Map_Items_Correctly()
    {
        RappiOrder source = TestDataFactory.CreateMinimalRappiOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.NotEmpty(result.Items);
        Assert.Equal("item-1", result.Items[0].Id);
        Assert.Equal("Pizza Calabresa", result.Items[0].Name);
        Assert.Equal(2, result.Items[0].Quantity);
        Assert.Equal("SKU-001", result.Items[0].ExternalCode);
    }

    [Fact]
    public void ToOrder_Should_Map_Delivery_Correctly()
    {
        RappiOrder source = TestDataFactory.CreateMinimalRappiOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.NotNull(result.Delivery);
        Assert.Equal("Av Paulista, 1000", result.Delivery!.DeliveryAddress?.FormattedAddress);
        Assert.Equal("São Paulo", result.Delivery!.DeliveryAddress?.City);
        Assert.Equal("Bela Vista", result.Delivery!.DeliveryAddress?.District);
    }

    [Fact]
    public void ToOrder_Should_Map_TenantId_Correctly()
    {
        RappiOrder source = TestDataFactory.CreateMinimalRappiOrder();

        Order result = source.ToOrder("custom-tenant");

        Assert.Equal("custom-tenant", result.TenantId);
    }

    [Fact]
    public void ToOrder_Should_Set_SourceAppId_To_RAPPI()
    {
        RappiOrder source = TestDataFactory.CreateMinimalRappiOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.Equal(RappiIntegrationKey.RAPPI, result.SourceAppId);
    }

    [Fact]
    public void ToOrder_Should_Map_OrderTiming_To_Instant()
    {
        RappiOrder source = TestDataFactory.CreateMinimalRappiOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.Equal(OrderTiming.INSTANT, result.OrderTiming);
    }

    [Fact]
    public void ToOrder_Should_Map_OrderType_Delivery()
    {
        RappiOrder source = TestDataFactory.CreateMinimalRappiOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.Equal(OrderType.DELIVERY, result.Type);
    }

    [Fact]
    public void ToOrder_Should_Map_Totals_Correctly()
    {
        RappiOrder source = TestDataFactory.CreateMinimalRappiOrder();

        Order result = source.ToOrder("tenant-1");

        Assert.NotNull(result.Total);
        Assert.Equal(55.00m, result.Total!.OrderAmount?.Value);
        Assert.Equal(50.00m, result.Total!.ItemsPrice?.Value);
        Assert.Equal(5.00m, result.Total!.Discount?.Value);
    }
}
