using System.Globalization;
using Patterns.Shop;

namespace Patterns.Modern.Options.Problem;

// Guide: §7.2
public sealed class ShippingSettings(Dictionary<string, string> raw)
{
    // PAIN: string keys and parsing in every method. A typo ("Shiping:StandardCost") or a bad value fails only
    // when the line runs, maybe in production, maybe weeks after the deployment. Nothing is checked at start-up.
    public decimal StandardCost() => decimal.Parse(raw["Shipping:StandardCost"], CultureInfo.InvariantCulture);

    public decimal Threshold() => decimal.Parse(raw["Shipping:FreeShippingThreshold"], CultureInfo.InvariantCulture);

    // PAIN: an array in configuration is one key per index; every reader has to know that.
    public IReadOnlyList<string> Countries() =>
        [.. raw.Where(kv => kv.Key.StartsWith("Shipping:Countries:", StringComparison.Ordinal))
               .OrderBy(kv => int.Parse(kv.Key["Shipping:Countries:".Length..], CultureInfo.InvariantCulture))
               .Select(kv => kv.Value)];

    public decimal CostFor(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        // PAIN: the same keys and the same parse again, here and in every other class that needs a setting.
        var threshold = decimal.Parse(raw["Shipping:FreeShippingThreshold"], CultureInfo.InvariantCulture);
        return order.Total >= threshold ? 0.00m : decimal.Parse(raw["Shipping:StandardCost"], CultureInfo.InvariantCulture);
    }
}
