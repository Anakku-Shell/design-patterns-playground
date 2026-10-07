using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Patterns.Demo;
using Patterns.Modern.Options.DotNet;
using Patterns.Shop;
using static System.FormattableString;

namespace Patterns.Modern.Options;

public sealed class OptionsDemo : IDemo
{
    public string Key => "options";
    public string Name => "Options";
    public PatternCategory Category => PatternCategory.Modern;
    public Relevance Relevance => Relevance.Essential;
    public string GuideSection => "§7.2";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var raw = new Dictionary<string, string>
        {
            ["Shipping:StandardCost"] = "4.99",
            ["Shipping:FreeShippingThreshold"] = "50.00",
            ["Shipping:Countries:0"] = "ES",
            ["Shipping:Countries:1"] = "PT",
            ["Shipping:Countries:2"] = "FR",
        };
        var threeBooks = SampleData.OrderOf((SampleData.Book, 3)); // 37.50

        narrator.Level(0, "Problem");
        narrator.Step("ShippingSettings: decimal.Parse(raw[\"Shipping:StandardCost\"]) in every method");
        narrator.Result(Invariant($"shipping for 37.50: {new Problem.ShippingSettings(raw).CostFor(threeBooks):0.00}"));
        narrator.Step("a typo in a key (\"Shiping:StandardCost\"): construction is fine, the first use fails");
        var typo = new Dictionary<string, string>(raw);
        typo.Remove("Shipping:StandardCost");
        typo["Shiping:StandardCost"] = "4.99";
        try
        {
            new Problem.ShippingSettings(typo).StandardCost();
        }
        catch (KeyNotFoundException e)
        {
            narrator.Result(e.Message);
        }

        narrator.Level(1, "Classic");
        narrator.Step("ShippingOptionsReader.Read(raw): parse and validate once, then pass typed ShippingOptions around");
        var options = Classic.ShippingOptionsReader.Read(raw);
        narrator.Result(Invariant($"StandardCost {options.StandardCost:0.00}, threshold {options.FreeShippingThreshold:0.00}, countries {string.Join(", ", options.Countries)}"));
        narrator.Result(Invariant($"shipping for 37.50: {new Classic.ShippingCalculator(options).CostFor(threeBooks):0.00}"));
        narrator.Step("a threshold of 0 is refused when read");
        try
        {
            Classic.ShippingOptionsReader.Read(new Dictionary<string, string>(raw) { ["Shipping:FreeShippingThreshold"] = "0" });
        }
        catch (ArgumentException e)
        {
            narrator.Result(e.Message);
        }

        narrator.Level(2, ".NET");
        narrator.Step("AddOptions<ShippingOptions>().Bind(config.GetSection(\"Shipping\")).Validate(...).ValidateOnStart()");
        var source = new SettableConfigurationSource(raw.Select(kv => new KeyValuePair<string, string?>(kv.Key, kv.Value)));
        using var services = new ServiceCollection()
            .AddShippingOptions(new ConfigurationBuilder().Add(source).Build())
            .BuildServiceProvider();
        narrator.Result(Invariant($"shipping for 37.50: {services.GetRequiredService<ShippingCalculator>().CostFor(threeBooks):0.00}"));
        narrator.Step("operations change StandardCost to 5.99 while the program runs");
        var once = services.GetRequiredService<IOptions<DotNet.ShippingOptions>>();
        var monitor = services.GetRequiredService<IOptionsMonitor<DotNet.ShippingOptions>>();
        _ = (once.Value, monitor.CurrentValue); // both read 4.99 before the change
        source.Provider.Set("Shipping:StandardCost", "5.99");
        narrator.Result(Invariant($"IOptions.Value.StandardCost = {once.Value.StandardCost:0.00} (read once, cached)"));
        narrator.Result(Invariant($"IOptionsMonitor.CurrentValue.StandardCost = {monitor.CurrentValue.StandardCost:0.00} (always the latest)"));

        narrator.Takeaway("Bind settings to a typed class, validate them once, and inject the accessor that matches how often they change.");
    }
}
