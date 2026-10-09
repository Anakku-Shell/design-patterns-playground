using System.Diagnostics.CodeAnalysis;
using Patterns.Shop;

namespace Patterns.Behavioral.Iterator.Problem;

// Guide: §6.4
public sealed class OrderHistory
{
    // PAIN: the storage is public; callers depend on it being a List (they call GetRange on it).
    [SuppressMessage("Design", "CA1002:Do not expose generic lists",
        Justification = "The pain this level shows: the analyzer flags exactly this leak.")]
    public List<Order> Orders { get; } = [];
}

/// <summary>A caller: the order history page. Every caller like this one repeats the paging arithmetic.</summary>
public sealed class HistoryScreen
{
    public IReadOnlyList<IReadOnlyList<Order>> Pages(OrderHistory history, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(history);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        var pages = new List<IReadOnlyList<Order>>();
        for (var start = 0; start < history.Orders.Count; start += pageSize)
        {
            // PAIN: Math.Min and the bounds are easy to get wrong (an off-by-one here loses the last page).
            pages.Add(history.Orders.GetRange(start, Math.Min(pageSize, history.Orders.Count - start)));
        }
        return pages;
    }
}
