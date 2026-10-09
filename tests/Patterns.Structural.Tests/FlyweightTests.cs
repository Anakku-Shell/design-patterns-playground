using Patterns.Structural.Flyweight.Classic;
using Patterns.Structural.Flyweight.DotNet;
using Xunit;

namespace Patterns.Structural.Tests;

public sealed class FlyweightTests
{
    [Fact]
    public void TenThousandEntries_ShareThreeCategoryObjects()
    {
        var factory = new CategoryInfoFactory();
        string[] categories = ["Books", "electronics", "HOME"];

        var entries = Enumerable.Range(1, 10_000)
            .Select(i => new CatalogEntry($"SKU-{i}", 10m, factory.Get(categories[i % 3])))
            .ToList();

        Assert.Equal(3, factory.Created);
        Assert.Same(entries[2].Category, entries[5].Category); // two Books entries, one object
    }

    [Fact]
    public void Get_IsCaseInsensitive_AndKeepsTheCanonicalName()
    {
        var factory = new CategoryInfoFactory();

        var books = factory.Get("books");

        Assert.Same(books, factory.Get("BOOKS"));
        Assert.Equal(new CategoryInfo("Books", "📚", 0.04m), books);
    }

    [Fact]
    public void Get_UnknownCategory_Throws()
    {
        Assert.Equal("Unknown category 'Toys'.",
            Assert.Throws<ArgumentException>(() => new CategoryInfoFactory().Get("Toys")).Message);
    }

    [Fact]
    public void Intern_ReturnsTheSameReference()
    {
        Assert.NotSame(SkuText.Build("BOOK-", "001"), SkuText.Build("BOOK-", "001"));
        Assert.Same(SkuText.BuildInterned("BOOK-", "001"), SkuText.BuildInterned("BOOK-", "001"));
    }
}
