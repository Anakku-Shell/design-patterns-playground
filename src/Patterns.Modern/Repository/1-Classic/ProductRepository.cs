using Patterns.Shop;

namespace Patterns.Modern.Repository.Classic;

// Role: Repository — products, as the business code thinks of them; not a word about storage.
// Guide: §7.3
public interface IProductRepository
{
    Product? GetById(Guid id);

    IReadOnlyList<Product> ListByCategory(string category);

    void Add(Product product);
}

// Role: ConcreteRepository — keeps products in memory (an EfProductRepository in a real application).
public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<Guid, Product> _products = [];

    public Product? GetById(Guid id) => _products.GetValueOrDefault(id);

    // Materialised: callers get a list, not a query that would run again later.
    public IReadOnlyList<Product> ListByCategory(string category) =>
        [.. _products.Values.Where(p => p.Category.Name.Equals(category, StringComparison.OrdinalIgnoreCase))];

    public void Add(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (!_products.TryAdd(product.Id, product))
        {
            throw new InvalidOperationException($"Product {product.Id} already exists.");
        }
    }
}

// Role: Client — business code that uses the interface only.
public sealed class CatalogService(IProductRepository products)
{
    public IReadOnlyList<Product> InCategory(string category) => products.ListByCategory(category);

    public decimal PriceOf(Guid id) =>
        products.GetById(id)?.Price ?? throw new KeyNotFoundException($"Product {id} not found.");
}
