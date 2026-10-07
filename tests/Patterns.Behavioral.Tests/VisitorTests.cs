using System.Linq.Expressions;
using Patterns.Behavioral.Visitor.Classic;
using Patterns.Behavioral.Visitor.DotNet;
using Patterns.Shop;
using Xunit;

namespace Patterns.Behavioral.Tests;

public sealed class VisitorTests
{
    private static BundleNode StarterKit() =>
        new("Starter kit",
        [
            new ProductNode(SampleData.Book),
            new ProductNode(SampleData.Mug),
            new BundleNode("Audio", [new ProductNode(SampleData.Headphones)]),
        ]);

    private static BundleItem StarterKitItems() =>
        new("Starter kit",
        [
            new ProductItem(SampleData.Book),
            new ProductItem(SampleData.Mug),
            new BundleItem("Audio", [new ProductItem(SampleData.Headphones)]),
        ]);

    [Fact]
    public void Vat_IsTheSameInBothLevels()
    {
        // 12.50 × 0.04 + 8.00 × 0.21 + 59.90 × 0.21 = 0.50 + 1.68 + 12.579 = 14.759 → 14.76
        var visitor = new VatVisitor();
        StarterKit().Accept(visitor);

        Assert.Equal(14.76m, visitor.Total);
        Assert.Equal(14.76m, VatCalculator.VatOf(StarterKitItems()));
    }

    [Fact]
    public void Vat_OfASingleProduct()
    {
        var visitor = new VatVisitor();
        new ProductNode(SampleData.Book).Accept(visitor);

        Assert.Equal(0.50m, visitor.Total);
        Assert.Equal(0.50m, VatCalculator.VatOf(new ProductItem(SampleData.Book)));
    }

    private static readonly Product Sticker =
        new(Guid.Empty, "Sticker", "HOME-002", 0.50m, SampleData.Home, 100); // VAT 0.105

    [Fact]
    public void Vat_RoundsHalfAwayFromZero()
    {
        var visitor = new VatVisitor();
        new ProductNode(Sticker).Accept(visitor);

        Assert.Equal(0.11m, visitor.Total); // to even would give 0.10
        Assert.Equal(0.11m, VatCalculator.VatOf(new ProductItem(Sticker)));
    }

    [Fact]
    public void Vat_IsRoundedOnceAtTheEnd()
    {
        // 0.105 + 0.105 = 0.21; rounding each product first would give 0.11 + 0.11 = 0.22.
        var visitor = new VatVisitor();
        new BundleNode("Two stickers", [new ProductNode(Sticker), new ProductNode(Sticker)]).Accept(visitor);

        Assert.Equal(0.21m, visitor.Total);
        Assert.Equal(0.21m, VatCalculator.VatOf(new BundleItem("Two stickers", [new ProductItem(Sticker), new ProductItem(Sticker)])));
    }

    [Fact]
    public void Outline_IsExact()
    {
        var visitor = new OutlineVisitor();

        StarterKit().Accept(visitor);

        Assert.Equal(
            "Starter kit\n" +
            "  Clean Code 12.50\n" +
            "  Coffee Mug 8.00\n" +
            "  Audio\n" +
            "    Wireless Headphones 59.90",
            visitor.Text);
    }

    [Fact]
    public void Outline_IgnoresTheCurrentCulture()
    {
        var text = CultureScope.WithDecimalComma(() =>
        {
            var visitor = new OutlineVisitor();
            new ProductNode(SampleData.Book).Accept(visitor);
            return visitor.Text;
        });

        Assert.Equal("Clean Code 12.50", text);
    }

    [Fact]
    public void ExpressionVisitor_FindsTheConstants()
    {
        Expression<Func<Order, bool>> rule = o => o.Total > 100m && o.Units < 5;
        var collector = new ConstantCollector();

        collector.Visit(rule);

        Assert.Equal([100m, 5], collector.Constants);
    }
}
