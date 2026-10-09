using System.Collections.Immutable;
using Patterns.Shop;

namespace Patterns.Behavioral.Memento.DotNet;

/// <summary>
/// An immutable cart: every change returns a new state and leaves the old one untouched, so the state is its
/// own memento and undo is a <c>Stack&lt;CartState&gt;</c>. Guide: §6.6.
/// </summary>
public sealed record CartState(ImmutableDictionary<Guid, int> Items)
{
    public static CartState Empty { get; } = new(ImmutableDictionary<Guid, int>.Empty);

    public CartState Add(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        return this with { Items = Items.SetItem(product.Id, Items.GetValueOrDefault(product.Id) + quantity) };
    }

    public CartState Remove(Guid productId) => this with { Items = Items.Remove(productId) };
}
