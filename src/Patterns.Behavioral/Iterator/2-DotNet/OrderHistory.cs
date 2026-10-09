using Patterns.Shop;

namespace Patterns.Behavioral.Iterator.DotNet;

/// <summary>
/// The same history with <c>yield return</c>: the compiler writes the iterator class. The source is kept as an
/// <see cref="IEnumerable{T}"/> (it could be a database read), so pages are built only as they are asked for.
/// Guide: §6.4.
/// </summary>
public sealed class OrderHistory(IEnumerable<Order> orders)
{
    public IEnumerable<IReadOnlyList<Order>> Pages(int pageSize)
    {
        // Checked here, not in the iterator method: code there would wait for the first MoveNext().
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);
        return PagesCore(pageSize);
    }

    private IEnumerable<IReadOnlyList<Order>> PagesCore(int pageSize)
    {
        var page = new List<Order>(pageSize);
        foreach (var order in orders)
        {
            page.Add(order);
            if (page.Count == pageSize)
            {
                yield return page; // pauses here until the caller asks for the next page
                page = new List<Order>(pageSize);
            }
        }
        if (page.Count > 0) { yield return page; }
    }
}
