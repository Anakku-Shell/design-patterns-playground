using System.Globalization;
using Patterns.Creational.Builder.Classic;
using Patterns.Creational.Builder.DotNet;
using Patterns.Shop;
using Xunit;
using Problem = Patterns.Creational.Builder.Problem;

namespace Patterns.Creational.Tests;

public sealed class BuilderTests
{
    [Fact]
    public void ProblemAndClassic_BuildTheSameOrder()
    {
        var mutable = new Problem.MutableOrder { Customer = SampleData.Ana, ShippingAddress = SampleData.Madrid };
        mutable.Lines.Add(new OrderLine(SampleData.Book, 2));
        mutable.Lines.Add(new OrderLine(SampleData.Headphones, 1));

        var built = OrderBuilder.For(SampleData.Ana)
            .ShipTo(SampleData.Madrid)
            .Add(SampleData.Book, 2)
            .Add(SampleData.Headphones)
            .Build();

        Assert.True(mutable.IsValid());
        Assert.Equal(mutable.Lines, built.Order.Lines);
        Assert.Equal(84.90m, mutable.Total);
        Assert.Equal(84.90m, built.Order.Total);
    }

    [Fact]
    public void Problem_InvalidOrderExistsUntilSomeoneChecks()
    {
        var order = new Problem.MutableOrder { Customer = SampleData.Ana }; // no address, no lines: still an object

        Assert.False(order.IsValid());
    }

    [Fact]
    public void Build_WithoutAddress_Throws()
    {
        var builder = OrderBuilder.For(SampleData.Ana).Add(SampleData.Book);

        Assert.Equal("An order needs a shipping address.", Assert.Throws<InvalidOperationException>(builder.Build).Message);
    }

    [Fact]
    public void Build_WithoutLines_Throws()
    {
        var builder = OrderBuilder.For(SampleData.Ana).ShipTo(SampleData.Madrid);

        Assert.Equal("An order needs at least one line.", Assert.Throws<InvalidOperationException>(builder.Build).Message);
    }

    [Fact]
    public void Add_SameProductTwice_MergesQuantities()
    {
        var built = OrderBuilder.For(SampleData.Ana).ShipTo(SampleData.Madrid)
            .Add(SampleData.Book, 1)
            .Add(SampleData.Book, 3)
            .Build();

        var line = Assert.Single(built.Order.Lines);
        Assert.Equal(4, line.Quantity);
    }

    [Fact]
    public void Add_QuantityBelowOne_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => OrderBuilder.For(SampleData.Ana).Add(SampleData.Book, 0));
    }

    [Fact]
    public void GiftNote_TravelsWithTheOrder()
    {
        var built = OrderBuilder.For(SampleData.Ana).ShipTo(SampleData.Madrid).Add(SampleData.Book)
            .WithGiftNote("Happy birthday")
            .Build();

        Assert.Equal("Happy birthday", built.GiftNote);
    }

    [Fact]
    public void Receipt_IsExact()
    {
        var order = AnOrder.WithLines((SampleData.Book, 2), (SampleData.Headphones, 1)).Build();

        // AppendLine ends lines with Environment.NewLine ("\r\n" on Windows); compare with "\n" everywhere.
        Assert.Equal("""
            Order for Ana
            2 x Clean Code @ 12.50 = 25.00
            1 x Wireless Headphones @ 59.90 = 59.90
            Total: 84.90
            """.ReplaceLineEndings("\n"), ReceiptPrinter.Print(order).ReplaceLineEndings("\n"));
    }

    [Fact]
    public void Receipt_IgnoresTheCurrentCulture()
    {
        var order = AnOrder.WithLines((SampleData.Book, 2)).Build();

        var receipt = WithDecimalComma(() => ReceiptPrinter.Print(order));

        Assert.Contains("2 x Clean Code @ 12.50 = 25.00", receipt, StringComparison.Ordinal);
    }

    [Fact]
    public void TrackingLink_HasPathAndQuery()
    {
        var id = new Guid("0a000000-0000-0000-0000-000000000001");

        Assert.Equal(new Uri("https://track.example.com/orders/0a000000-0000-0000-0000-000000000001?lang=en"), TrackingLink.For(id));
    }

    [Fact]
    public void TestDataBuilder_DefaultsToAnaInMadrid()
    {
        var order = AnOrder.WithLines((SampleData.Book, 4)).ShippedTo(SampleData.Lisbon).Build();

        Assert.Equal(SampleData.Ana, order.Customer);
        Assert.Equal(SampleData.Lisbon, order.ShippingAddress);
        Assert.Equal(50.00m, order.Total);
    }

    // The repository runs with invariant globalization, so the current culture is always invariant and a
    // missing CultureInfo.InvariantCulture would go unnoticed. A cloned culture with a decimal comma (like
    // es-ES) still works in that mode and makes the mistake visible.
    private static T WithDecimalComma<T>(Func<T> action)
    {
        var previous = CultureInfo.CurrentCulture;
        var comma = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        comma.NumberFormat.NumberDecimalSeparator = ",";
        comma.NumberFormat.NumberGroupSeparator = ".";
        CultureInfo.CurrentCulture = comma;
        try
        {
            return action();
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }
}
