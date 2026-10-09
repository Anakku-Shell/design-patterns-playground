using Patterns.Shop;

namespace Patterns.Modern.Repository.Problem;

// Guide: §7.3
public sealed class CatalogService(IList<Product> table) // the list stands in for a database table
{
    // PAIN: storage details and queries are repeated inside business methods. Moving to a database means
    // rewriting every method that touches the list.
    public IReadOnlyList<Product> Books() =>
        [.. table.Where(p => p.Category.Name.Equals("Books", StringComparison.OrdinalIgnoreCase))];

    public IReadOnlyList<Product> InCategory(string category) =>
        [.. table.Where(p => p.Category.Name.Equals(category, StringComparison.OrdinalIgnoreCase))];

    public decimal PriceOf(Guid id) => table.First(p => p.Id == id).Price;

    // PAIN: nothing stops a second product with the same id; every method that adds has to remember to check.
    public void AddProduct(Product product) => table.Add(product);
}
