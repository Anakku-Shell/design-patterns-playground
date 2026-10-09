using System.Collections.Immutable;
using Patterns.Shop;

namespace Patterns.Behavioral.Memento.Classic;

// Role: Memento — an opaque snapshot of a cart. Nothing outside this assembly can read or create it.
// Guide: §6.6
public sealed class CartSnapshot
{
    internal CartSnapshot(ImmutableDictionary<Guid, int> items) => Items = items;

    internal ImmutableDictionary<Guid, int> Items { get; }
}

// Role: Originator — the only one who knows how to save and restore itself.
public sealed class Cart
{
    private ImmutableDictionary<Guid, int> _items = ImmutableDictionary<Guid, int>.Empty;

    public IReadOnlyDictionary<Guid, int> Items => _items;

    public void Add(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        _items = _items.SetItem(product.Id, _items.GetValueOrDefault(product.Id) + quantity);
    }

    public void Remove(Guid productId) => _items = _items.Remove(productId);

    public CartSnapshot CreateSnapshot() => new(_items); // immutable: later edits cannot change it

    public void Restore(CartSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        _items = snapshot.Items;
    }
}

// Role: Caretaker — keeps the snapshots, in order, without knowing what is inside.
public sealed class CartCaretaker(Cart cart)
{
    private readonly Stack<CartSnapshot> _history = new();

    public void Save() => _history.Push(cart.CreateSnapshot());

    /// <summary>Restores the last saved snapshot; false when nothing was saved.</summary>
    public bool Undo()
    {
        if (!_history.TryPop(out var snapshot)) { return false; }
        cart.Restore(snapshot);
        return true;
    }
}
