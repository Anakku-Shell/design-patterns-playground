namespace Patterns.Creational.Singleton.Problem;

// Guide: §4.1
public static class VatRates
{
    // PAIN: public and mutable. Any code (and any test) can change a rate for everybody, and tests that
    // run in parallel see each other's changes.
    public static Dictionary<string, decimal> Rates { get; } = new()
    {
        ["ES"] = 0.21m,
        ["PT"] = 0.23m,
        ["FR"] = 0.20m,
    };

    // PAIN: callers reach this through the type name; nothing in their constructors says they depend on it.
    public static decimal RateFor(string countryCode)
    {
        ArgumentNullException.ThrowIfNull(countryCode);
        var code = countryCode.Trim().ToUpperInvariant();
        return Rates.TryGetValue(code, out var rate)
            ? rate
            : throw new ArgumentException($"No VAT rate for country '{code}'.");
    }
}
