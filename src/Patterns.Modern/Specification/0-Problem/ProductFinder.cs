using Patterns.Shop;

namespace Patterns.Modern.Specification.Problem;

// Guide: §7.5
public sealed class ProductFinder(IReadOnlyList<Product> products)
{
    // PAIN: one method per combination. Three rules already allow dozens of combinations, and each method
    // repeats the definition of "in stock" or "in a category"; next week marketing asks for another one.
    public IReadOnlyList<Product> FindInStockBooks() =>
        [.. products.Where(p => p.Stock > 0 && p.Category.Name.Equals("Books", StringComparison.OrdinalIgnoreCase))];

    public IReadOnlyList<Product> FindCheapInStock(decimal limit) =>
        [.. products.Where(p => p.Stock > 0 && p.Price < limit)];

    public IReadOnlyList<Product> FindOutOfStock() =>
        [.. products.Where(p => p.Stock <= 0)];

    public IReadOnlyList<Product> FindBooksOrHome() =>
        [.. products.Where(p => p.Category.Name.Equals("Books", StringComparison.OrdinalIgnoreCase)
                             || p.Category.Name.Equals("Home", StringComparison.OrdinalIgnoreCase))];
}
