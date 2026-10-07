using Microsoft.Extensions.Configuration;
using Patterns.Shop;
using Patterns.Structural.Composite.Classic;
using Patterns.Structural.Composite.DotNet;
using Xunit;
using Problem = Patterns.Structural.Composite.Problem;

namespace Patterns.Structural.Tests;

public sealed class CompositeTests
{
    [Fact]
    public void ProblemAndClassic_AnswerAlikeForTheStarterKit()
    {
        var audio = new Problem.CatalogEntry();
        audio.Children.Add(new Problem.CatalogEntry { Product = SampleData.Headphones });
        var kit = new Problem.CatalogEntry();
        kit.Children.Add(new Problem.CatalogEntry { Product = SampleData.Book });
        kit.Children.Add(new Problem.CatalogEntry { Product = SampleData.Mug });
        kit.Children.Add(audio);

        Assert.Equal(80.40m, Problem.CatalogEntry.PriceOf(kit));
        Assert.Equal(80.40m, StarterKit().Price);
        Assert.Equal(3, Problem.CatalogEntry.ProductCountOf(kit));
        Assert.Equal(3, StarterKit().ProductCount);
    }

    [Fact]
    public void ProductCount_CountsNestedProducts()
    {
        var outer = new Bundle("Outer");
        outer.Add(StarterKit());
        outer.Add(new ProductItem(SampleData.Book));

        Assert.Equal(4, outer.ProductCount); // 3 inside the kit, two levels down, plus the book
    }

    [Fact]
    public void EmptyBundle_CostsZero()
    {
        Assert.Equal(0.00m, new Bundle("Empty").Price);
    }

    [Fact]
    public void Bundle_CannotContainItself()
    {
        var kit = new Bundle("Starter kit");

        Assert.Equal("A bundle cannot contain itself.",
            Assert.Throws<InvalidOperationException>(() => kit.Add(kit)).Message);
    }

    [Fact]
    public void Bundle_CannotContainItsParent()
    {
        var kit = new Bundle("Starter kit");
        var audio = new Bundle("Audio");
        var inner = new Bundle("Inner");
        kit.Add(audio);
        audio.Add(inner);

        // inner → audio → kit: adding kit under inner would close a cycle two levels deep.
        Assert.Equal("A bundle cannot contain itself.",
            Assert.Throws<InvalidOperationException>(() => inner.Add(kit)).Message);
        Assert.Throws<InvalidOperationException>(() => audio.Add(kit));
    }

    [Fact]
    public void DotNet_ConfigurationChildren_AreRead()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Shipping:Carriers:Fast:Days"] = "1",
                ["Shipping:Carriers:Slow:Days"] = "5",
            })
            .Build();

        Assert.Equal(new Dictionary<string, int> { ["Fast"] = 1, ["Slow"] = 5 }, CarrierSettings.Read(configuration));
    }

    private static Bundle StarterKit()
    {
        var audio = new Bundle("Audio");
        audio.Add(new ProductItem(SampleData.Headphones));
        var kit = new Bundle("Starter kit");
        kit.Add(new ProductItem(SampleData.Book));
        kit.Add(new ProductItem(SampleData.Mug));
        kit.Add(audio);
        return kit;
    }
}
