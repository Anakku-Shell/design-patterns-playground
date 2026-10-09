using Microsoft.Extensions.DependencyInjection;
using Patterns.Behavioral.Strategy.DotNet;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Behavioral.Strategy.Classic;
using DotNet = Patterns.Behavioral.Strategy.DotNet;
using Problem = Patterns.Behavioral.Strategy.Problem;

namespace Patterns.Behavioral.Tests;

public sealed class StrategyTests
{
    private static readonly Order ThreeBooks = SampleData.OrderOf((SampleData.Book, 3)); // 37.50, 3 units
    private static readonly Order FourBooks = SampleData.OrderOf((SampleData.Book, 4));  // exactly 50.00

    private static Classic.IShippingStrategy ClassicStrategy(string method) => method switch
    {
        "standard" => new Classic.StandardShipping(),
        "express" => new Classic.ExpressShipping(),
        "pickup" => new Classic.StorePickup(),
        _ => throw new ArgumentOutOfRangeException(nameof(method)),
    };

    private static Func<Order, decimal> Lambda(string method) => method switch
    {
        "standard" => ShippingRules.Standard,
        "express" => ShippingRules.Express,
        "pickup" => ShippingRules.Pickup,
        _ => throw new ArgumentOutOfRangeException(nameof(method)),
    };

    private static ServiceProvider Services() => new ServiceCollection().AddShippingStrategies().BuildServiceProvider();

    [Theory]
    [InlineData("standard", 4.99)]
    [InlineData("express", 12.99)]
    [InlineData("pickup", 0.00)]
    public void AllLevels_PriceThreeBooksTheSame(string method, decimal expected)
    {
        using var services = Services();

        Assert.Equal(expected, new Problem.ShippingCalculator().CostFor(ThreeBooks, method));
        Assert.Equal(expected, new Classic.ShippingCalculator(ClassicStrategy(method)).CostFor(ThreeBooks));
        Assert.Equal(expected, new DotNet.ShippingCalculator(services).CostFor(ThreeBooks, method));
        Assert.Equal(expected, Lambda(method)(ThreeBooks));
    }

    [Fact]
    public void Standard_AtExactly50_IsFree()
    {
        using var services = Services();

        Assert.Equal(0.00m, new Problem.ShippingCalculator().CostFor(FourBooks, "standard"));
        Assert.Equal(0.00m, new Classic.StandardShipping().CostFor(FourBooks));
        Assert.Equal(0.00m, new DotNet.ShippingCalculator(services).CostFor(FourBooks, "standard"));
        Assert.Equal(0.00m, ShippingRules.Standard(FourBooks));
    }

    [Fact]
    public void Problem_UnknownMethod_Throws()
    {
        var error = Assert.Throws<ArgumentException>(() => new Problem.ShippingCalculator().CostFor(ThreeBooks, "drone"));

        Assert.Equal("Unknown shipping method 'drone'.", error.Message);
    }

    [Fact]
    public void DotNet_UnknownKey_Throws()
    {
        using var services = Services();

        Assert.Throws<InvalidOperationException>(() => new DotNet.ShippingCalculator(services).CostFor(ThreeBooks, "drone"));
    }

    [Fact]
    public void DotNet_LambdaStrategy_GivesTheSameResult()
    {
        using var services = Services();
        var calculator = new DotNet.ShippingCalculator(services);

        Assert.Equal(calculator.CostFor(ThreeBooks, "express"), ShippingRules.Express(ThreeBooks));
        Assert.Equal(50.49m, ShippingRules.PriceWith(ThreeBooks, ShippingRules.Express)); // 37.50 + 12.99
    }
}
