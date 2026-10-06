namespace Patterns.Creational.Singleton.Classic;

// Role: Singleton — the one VAT rate table of the application.
// Guide: §4.1
public sealed class VatRateTable
{
    // Lazy<T> creates the instance on first use and is thread-safe by default
    // (LazyThreadSafetyMode.ExecutionAndPublication): two threads never build two tables.
    private static readonly Lazy<VatRateTable> LazyInstance = new(() => new VatRateTable());

    private readonly Dictionary<string, decimal> _rates = new()
    {
        ["ES"] = 0.21m,
        ["PT"] = 0.23m,
        ["FR"] = 0.20m,
    };

    // Private: nobody else can call new VatRateTable(). That is what makes it a Singleton.
    private VatRateTable()
    {
    }

    public static VatRateTable Instance => LazyInstance.Value;

    public decimal RateFor(string countryCode)
    {
        ArgumentNullException.ThrowIfNull(countryCode);
        var code = countryCode.Trim().ToUpperInvariant();
        return _rates.TryGetValue(code, out var rate)
            ? rate
            // No paramName: it would append " (Parameter '…')" to the message.
            : throw new ArgumentException($"No VAT rate for country '{code}'.");
    }
}
