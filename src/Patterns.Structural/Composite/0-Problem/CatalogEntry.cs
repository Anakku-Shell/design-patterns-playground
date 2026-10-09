using Patterns.Shop;

namespace Patterns.Structural.Composite.Problem;

// Guide: §5.3
public sealed class CatalogEntry
{
    // Set for a single product…
    public Product? Product { get; init; }

    // …or children, for a bundle. Nothing stops an entry from having both.
    public IList<CatalogEntry> Children { get; } = new List<CatalogEntry>();

    public static decimal PriceOf(CatalogEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        // PAIN: every operation (price, count, export…) repeats this "is it one or many?" check.
        if (entry.Product is not null) { return entry.Product.Price; }
        decimal total = 0;
        foreach (var child in entry.Children) { total += PriceOf(child); }
        return total;
    }

    public static int ProductCountOf(CatalogEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        // PAIN: the same check again, for the second operation.
        if (entry.Product is not null) { return 1; }
        return entry.Children.Sum(ProductCountOf);
    }
}
