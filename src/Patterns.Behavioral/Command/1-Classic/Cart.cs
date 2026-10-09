using Patterns.Shop;

namespace Patterns.Behavioral.Command.Classic;

/// <summary>A shopping cart: product id → units. Guide: §6.2.</summary>
// Role: Receiver — does the real work; knows nothing about commands.
public sealed class Cart
{
    private readonly Dictionary<Guid, int> _items = [];

    public IReadOnlyDictionary<Guid, int> Items => _items;

    public void Add(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        _items[product.Id] = _items.GetValueOrDefault(product.Id) + quantity;
    }

    /// <summary>Removes the product with all its units; removing an absent product does nothing.</summary>
    public void Remove(Guid productId) => _items.Remove(productId);
}
