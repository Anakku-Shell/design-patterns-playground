namespace Patterns.Structural.Flyweight.Classic;

// Role: Flyweight — category data shared by every entry of that category. Immutable on purpose: many owners
// hold the same object, so one owner's change would show up in all the others.
// Guide: §5.6
public sealed record CategoryInfo(string Name, string Icon, decimal VatRate);

// Role: Client — the extrinsic state (SKU, price) plus a reference to the shared category.
public sealed record CatalogEntry(string Sku, decimal Price, CategoryInfo Category);

// Role: FlyweightFactory — creates each category once and hands out the shared instance.
public sealed class CategoryInfoFactory
{
    // Illustrative per-category rates for this example; the shop's VAT per country is in guide §4.1.
    private static readonly Dictionary<string, (string Icon, decimal VatRate)> Known =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Books"] = ("📚", 0.04m),
            ["Electronics"] = ("🔌", 0.21m),
            ["Home"] = ("🏠", 0.21m),
        };

    // "books" → "Books": the flyweight always carries the canonical name, whatever the caller typed.
    private static readonly Dictionary<string, string> CanonicalNames =
        Known.Keys.ToDictionary(key => key, StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<string, CategoryInfo> _cache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>How many flyweights this factory has created (not how many times it was asked).</summary>
    public int Created { get; private set; }

    public CategoryInfo Get(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (_cache.TryGetValue(name, out var shared)) { return shared; }
        if (!Known.TryGetValue(name, out var data))
        {
            // No paramName: it would append " (Parameter 'name')" to the message.
            throw new ArgumentException($"Unknown category '{name}'.");
        }

        Created++;
        return _cache[name] = new CategoryInfo(CanonicalNames[name], data.Icon, data.VatRate);
    }
}
