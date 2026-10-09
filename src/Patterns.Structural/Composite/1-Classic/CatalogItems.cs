using Patterns.Shop;

namespace Patterns.Structural.Composite.Classic;

// Role: Component — anything in the catalog: a single product or a bundle of items.
// Guide: §5.3
public interface ICatalogItem
{
    string Name { get; }
    decimal Price { get; }
    int ProductCount { get; }
}

// Role: Leaf — one product; answers from its own data.
public sealed class ProductItem(Product product) : ICatalogItem
{
    public string Name => product.Name;
    public decimal Price => product.Price;
    public int ProductCount => 1;
}

// Role: Composite — a bundle; answers every question by asking its children. Add lives here, not on the
// component ("safety"): the compiler stops you from adding children to a product.
public sealed class Bundle(string name) : ICatalogItem
{
    private readonly List<ICatalogItem> _items = [];

    public string Name => name;
    public IReadOnlyList<ICatalogItem> Items => _items;
    public decimal Price => _items.Sum(item => item.Price);
    public int ProductCount => _items.Sum(item => item.ProductCount);

    public void Add(ICatalogItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        // A cycle would make Price recurse forever: a StackOverflowException, which cannot be caught.
        if (ReferenceEquals(item, this) || (item is Bundle bundle && bundle.Contains(this)))
        {
            throw new InvalidOperationException("A bundle cannot contain itself.");
        }
        _items.Add(item);
    }

    private bool Contains(ICatalogItem target) =>
        _items.Any(item => ReferenceEquals(item, target) || (item is Bundle inner && inner.Contains(target)));
}
