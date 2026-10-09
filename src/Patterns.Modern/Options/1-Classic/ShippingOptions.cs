using System.Globalization;
using Patterns.Shop;

namespace Patterns.Modern.Options.Classic;

// Role: Options — the shipping settings, typed.
// Guide: §7.2
public sealed class ShippingOptions
{
    public decimal StandardCost { get; set; }

    public decimal FreeShippingThreshold { get; set; }

    public IReadOnlyList<string> Countries { get; set; } = [];
}

// Role: Source and validation — reads and checks once, at start-up; after that everybody gets a typed object.
public static class ShippingOptionsReader
{
    private const string CountriesPrefix = "Shipping:Countries:";

    public static ShippingOptions Read(IDictionary<string, string> raw)
    {
        ArgumentNullException.ThrowIfNull(raw);
        var options = new ShippingOptions
        {
            StandardCost = decimal.Parse(raw["Shipping:StandardCost"], CultureInfo.InvariantCulture),
            FreeShippingThreshold = decimal.Parse(raw["Shipping:FreeShippingThreshold"], CultureInfo.InvariantCulture),
            // Arrays in configuration are one key per index; sort by the number, not the text ("10" < "2").
            Countries = [.. raw.Where(kv => kv.Key.StartsWith(CountriesPrefix, StringComparison.Ordinal))
                               .OrderBy(kv => int.Parse(kv.Key[CountriesPrefix.Length..], CultureInfo.InvariantCulture))
                               .Select(kv => kv.Value)],
        };
        if (options.FreeShippingThreshold <= 0)
        {
            throw new ArgumentException("FreeShippingThreshold must be positive.");
        }
        if (options.Countries.Count == 0)
        {
            throw new ArgumentException("At least one shipping country is required.");
        }
        return options;
    }
}

// Role: Consumer — receives typed options; no keys, no parsing.
public sealed class ShippingCalculator(ShippingOptions options)
{
    public decimal CostFor(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return order.Total >= options.FreeShippingThreshold ? 0.00m : options.StandardCost;
    }
}
