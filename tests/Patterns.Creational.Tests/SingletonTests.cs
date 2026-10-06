using Microsoft.Extensions.DependencyInjection;
using Patterns.Creational.Singleton.DotNet;
using Xunit;
using Classic = Patterns.Creational.Singleton.Classic;
using Problem = Patterns.Creational.Singleton.Problem;

namespace Patterns.Creational.Tests;

public sealed class SingletonTests
{
    [Theory]
    [InlineData("ES", 0.21)]
    [InlineData("PT", 0.23)]
    [InlineData("FR", 0.20)]
    public void EveryLevel_GivesTheSameRates(string country, double expected)
    {
        var rate = (decimal)expected;

        Assert.Equal(rate, Problem.VatRates.RateFor(country));
        Assert.Equal(rate, Classic.VatRateTable.Instance.RateFor(country));
        Assert.Equal(rate, new VatRateTable().RateFor(country));
    }

    [Fact]
    public void Classic_Instance_IsAlwaysTheSameObject()
    {
        Assert.Same(Classic.VatRateTable.Instance, Classic.VatRateTable.Instance);
    }

    [Fact]
    public void DotNet_OneContainer_ResolvesTheSameObject()
    {
        using var provider = new ServiceCollection().AddVatRates().BuildServiceProvider();

        Assert.Same(provider.GetRequiredService<VatRateTable>(), provider.GetRequiredService<VatRateTable>());
    }

    [Fact]
    public void DotNet_TwoContainers_HaveDifferentObjects()
    {
        // "Singleton" in DI means one per container: each test can build its own and share nothing.
        using var first = new ServiceCollection().AddVatRates().BuildServiceProvider();
        using var second = new ServiceCollection().AddVatRates().BuildServiceProvider();

        Assert.NotSame(first.GetRequiredService<VatRateTable>(), second.GetRequiredService<VatRateTable>());
    }

    [Fact]
    public void RateFor_IsCaseAndSpaceInsensitive()
    {
        Assert.Equal(0.21m, Problem.VatRates.RateFor(" es "));
        Assert.Equal(0.21m, Classic.VatRateTable.Instance.RateFor(" es "));
        Assert.Equal(0.21m, new VatRateTable().RateFor(" es "));
    }

    [Fact]
    public void RateFor_UnknownCountry_Throws()
    {
        Assert.Equal("No VAT rate for country 'XX'.",
            Assert.Throws<ArgumentException>(() => Problem.VatRates.RateFor("xx")).Message);
        Assert.Equal("No VAT rate for country 'XX'.",
            Assert.Throws<ArgumentException>(() => Classic.VatRateTable.Instance.RateFor("xx")).Message);
        Assert.Equal("No VAT rate for country 'XX'.",
            Assert.Throws<ArgumentException>(() => new VatRateTable().RateFor("xx")).Message);
    }

    [Fact]
    public void Problem_AnyoneCanChangeTheRates()
    {
        // The test itself shows the pain: it must change global state and remember to put it back.
        try
        {
            Problem.VatRates.Rates["ES"] = 0.99m;

            Assert.Equal(0.99m, Problem.VatRates.RateFor("ES"));
        }
        finally
        {
            Problem.VatRates.Rates["ES"] = 0.21m;
        }
    }
}
