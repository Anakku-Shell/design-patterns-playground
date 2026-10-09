using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Patterns.Modern.Options.DotNet;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Modern.Options.Classic;
using DotNet = Patterns.Modern.Options.DotNet;
using Problem = Patterns.Modern.Options.Problem;

namespace Patterns.Modern.Tests;

public sealed class OptionsTests
{
    private static Dictionary<string, string?> Settings(string threshold = "50.00") => new()
    {
        ["Shipping:StandardCost"] = "4.99",
        ["Shipping:FreeShippingThreshold"] = threshold,
        ["Shipping:Countries:0"] = "ES",
        ["Shipping:Countries:1"] = "PT",
        ["Shipping:Countries:2"] = "FR",
    };

    private static Dictionary<string, string> Raw(string threshold = "50.00") =>
        Settings(threshold).ToDictionary(kv => kv.Key, kv => kv.Value!);

    private static ServiceProvider Services(IConfiguration configuration) =>
        new ServiceCollection().AddShippingOptions(configuration).BuildServiceProvider();

    private static IConfiguration InMemory(string threshold = "50.00") =>
        new ConfigurationBuilder().AddInMemoryCollection(Settings(threshold)).Build();

    [Fact]
    public void AllLevels_ReadTheSameValues()
    {
        var problem = new Problem.ShippingSettings(Raw());
        var classic = Classic.ShippingOptionsReader.Read(Raw());
        using var services = Services(InMemory());
        var dotnet = services.GetRequiredService<IOptions<DotNet.ShippingOptions>>().Value;

        Assert.Equal((4.99m, 50.00m), (problem.StandardCost(), problem.Threshold()));
        Assert.Equal((4.99m, 50.00m), (classic.StandardCost, classic.FreeShippingThreshold));
        Assert.Equal((4.99m, 50.00m), (dotnet.StandardCost, dotnet.FreeShippingThreshold));
        Assert.Equal(["ES", "PT", "FR"], problem.Countries());
        Assert.Equal(["ES", "PT", "FR"], classic.Countries);
        Assert.Equal(["ES", "PT", "FR"], dotnet.Countries);
    }

    [Theory]
    [InlineData(3, 4.99)] // 37.50
    [InlineData(4, 0.00)] // exactly 50.00: free
    public void AllLevels_PriceShippingTheSame(int books, decimal expected)
    {
        var order = SampleData.OrderOf((SampleData.Book, books));
        using var services = Services(InMemory());

        Assert.Equal(expected, new Problem.ShippingSettings(Raw()).CostFor(order));
        Assert.Equal(expected, new Classic.ShippingCalculator(Classic.ShippingOptionsReader.Read(Raw())).CostFor(order));
        Assert.Equal(expected, services.GetRequiredService<DotNet.ShippingCalculator>().CostFor(order));
    }

    [Fact]
    public void Problem_ATypoFailsOnlyWhenTheValueIsUsed()
    {
        var raw = Raw();
        raw.Remove("Shipping:StandardCost");
        raw["Shiping:StandardCost"] = "4.99"; // the typo

        var settings = new Problem.ShippingSettings(raw); // nothing is checked here

        Assert.Equal(50.00m, settings.Threshold());
        Assert.Throws<KeyNotFoundException>(() => settings.StandardCost());
    }

    [Fact]
    public void Classic_Invalid_FailsWhenRead()
    {
        var error = Assert.Throws<ArgumentException>(() => Classic.ShippingOptionsReader.Read(Raw(threshold: "0")));

        Assert.Equal("FreeShippingThreshold must be positive.", error.Message);
    }

    [Fact]
    public void Classic_NoCountries_FailsWhenRead()
    {
        var raw = Raw();
        raw.Remove("Shipping:Countries:0");
        raw.Remove("Shipping:Countries:1");
        raw.Remove("Shipping:Countries:2");

        var error = Assert.Throws<ArgumentException>(() => Classic.ShippingOptionsReader.Read(raw));

        Assert.Equal("At least one shipping country is required.", error.Message);
    }

    [Fact]
    public void Classic_ReadsCountriesInIndexOrder()
    {
        // Index 10 sorts before 2 as text; the reader must sort by number.
        var raw = Raw();
        raw["Shipping:Countries:10"] = "IT";

        Assert.Equal(["ES", "PT", "FR", "IT"], Classic.ShippingOptionsReader.Read(raw).Countries);
    }

    [Fact]
    public void Invalid_FailsWhenRead()
    {
        using var services = Services(InMemory(threshold: "0"));
        var options = services.GetRequiredService<IOptions<DotNet.ShippingOptions>>(); // resolving is fine

        var error = Assert.Throws<OptionsValidationException>(() => options.Value);

        Assert.Contains("FreeShippingThreshold must be positive.", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateOnStart_FailsWithoutReadingTheOptions()
    {
        // What the generic host calls at start-up (IStartupValidator, .NET 8+); no one has read the options yet.
        using var services = Services(InMemory(threshold: "0"));

        var error = Assert.Throws<OptionsValidationException>(() => services.GetRequiredService<IStartupValidator>().Validate());

        Assert.Contains("FreeShippingThreshold must be positive.", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Monitor_SeesTheNewValue_OptionsDoesNot()
    {
        var source = new SettableConfigurationSource(Settings());
        using var services = Services(new ConfigurationBuilder().Add(source).Build());
        var options = services.GetRequiredService<IOptions<DotNet.ShippingOptions>>();
        var monitor = services.GetRequiredService<IOptionsMonitor<DotNet.ShippingOptions>>();
        Assert.Equal(4.99m, options.Value.StandardCost);
        Assert.Equal(4.99m, monitor.CurrentValue.StandardCost);

        source.Provider.Set("Shipping:StandardCost", "5.99");

        Assert.Equal(4.99m, options.Value.StandardCost);
        Assert.Equal(5.99m, monitor.CurrentValue.StandardCost);
    }
}
