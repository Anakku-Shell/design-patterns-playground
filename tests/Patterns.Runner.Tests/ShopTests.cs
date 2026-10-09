using Patterns.Shop;
using Xunit;

namespace Patterns.Runner.Tests;

public sealed class ShopTests
{
    [Fact]
    public void Order_Total_IsTheSumOfLines()
    {
        var order = SampleData.OrderOf((SampleData.Book, 2), (SampleData.Headphones, 1));

        Assert.Equal(84.90m, order.Total);
        Assert.Equal(3, order.Units);
    }

    [Fact]
    public void SampleData_HasAnOutOfStockProduct()
    {
        Assert.Equal(0, SampleData.Mug.Stock);
    }

    [Fact]
    public void OrderOf_IsADraftForAnaToMadrid()
    {
        var order = SampleData.OrderOf((SampleData.Book, 1));

        Assert.Equal(OrderStatus.Draft, order.Status);
        Assert.Equal(SampleData.Ana, order.Customer);
        Assert.Equal(SampleData.Madrid, order.ShippingAddress);
    }

    [Fact]
    public void SampleData_IdsAreFixed()
    {
        // Fixed ids keep demo output and test expectations the same on every run.
        Assert.Equal(new Guid("b0000000-0000-0000-0000-000000000001"), SampleData.Book.Id);
        Assert.Equal(new Guid("e0000000-0000-0000-0000-000000000001"), SampleData.Headphones.Id);
        Assert.Equal(new Guid("40000000-0000-0000-0000-000000000001"), SampleData.Mug.Id);
        Assert.Equal(new Guid("c0000000-0000-0000-0000-000000000001"), SampleData.Ana.Id);
        Assert.Equal(new Guid("c0000000-0000-0000-0000-000000000002"), SampleData.Guest.Id);
    }

    [Fact]
    public void OrderOf_GivesEachOrderANewId()
    {
        Assert.NotEqual(SampleData.OrderOf((SampleData.Book, 1)).Id, SampleData.OrderOf((SampleData.Book, 1)).Id);
    }
}
