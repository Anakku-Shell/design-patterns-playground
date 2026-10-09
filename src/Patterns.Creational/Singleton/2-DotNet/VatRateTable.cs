namespace Patterns.Creational.Singleton.DotNet;

// Role: Singleton — an ordinary class. "Only one" is the container's job (AddSingleton), not the class's.
// Guide: §4.1
public sealed class VatRateTable
{
    private readonly Dictionary<string, decimal> _rates = new()
    {
        ["ES"] = 0.21m,
        ["PT"] = 0.23m,
        ["FR"] = 0.20m,
    };

    public decimal RateFor(string countryCode)
    {
        ArgumentNullException.ThrowIfNull(countryCode);
        var code = countryCode.Trim().ToUpperInvariant();
        return _rates.TryGetValue(code, out var rate)
            ? rate
            : throw new ArgumentException($"No VAT rate for country '{code}'.");
    }
}
