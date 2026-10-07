using System.Diagnostics.CodeAnalysis;
using Patterns.Shop;

namespace Patterns.Modern.Repository.DotNet;

// Role: Repository — the IQueryable<T> shape a DbSet<T> gives you; the provider runs the query.
// Guide: §7.3
public sealed class ProductQueries(IQueryable<Product> products) // a DbSet<Product> in a real application
{
    // A case conversion, not a StringComparison overload: a query provider can translate a conversion to SQL
    // (EF Core turns ToUpper() into UPPER), but not a comparison overload. Over this in-memory source the
    // culture-safe ToUpperInvariant() is used.
    [SuppressMessage("Performance", "CA1862", Justification = "An IQueryable provider translates a case conversion to SQL, not a StringComparison overload.")]
    public IReadOnlyList<Product> InCategory(string category)
    {
        ArgumentNullException.ThrowIfNull(category);
        var wanted = category.ToUpperInvariant();
        return [.. products.Where(p => p.Category.Name.ToUpperInvariant() == wanted)];
    }
}
