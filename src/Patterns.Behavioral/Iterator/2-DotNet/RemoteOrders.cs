using System.Runtime.CompilerServices;
using Patterns.Shop;

namespace Patterns.Behavioral.Iterator.DotNet;

/// <summary>A remote API that returns orders a page at a time (1-based); it counts the calls it gets.</summary>
public sealed class FakeOrderApi
{
    private readonly List<Order> _orders;
    private readonly int _pageSize;

    public FakeOrderApi(IEnumerable<Order> orders, int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);
        _orders = [.. orders];
        _pageSize = pageSize;
    }

    public int Calls { get; private set; }

    /// <summary>The orders of page <paramref name="page"/>; an empty list past the last page.</summary>
    public async Task<IReadOnlyList<Order>> GetPageAsync(int page, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(page, 1);
        cancellationToken.ThrowIfCancellationRequested();
        await Task.Yield(); // a real API would await the network here
        Calls++;
        return [.. _orders.Skip((page - 1) * _pageSize).Take(_pageSize)];
    }
}

/// <summary><c>IAsyncEnumerable</c>: the iterator pattern when getting each element needs <c>await</c>. Guide: §6.4.</summary>
public static class RemoteOrders
{
    public static async IAsyncEnumerable<Order> StreamAsync(
        FakeOrderApi api, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(api);
        for (var page = 1; ; page++)
        {
            var orders = await api.GetPageAsync(page, cancellationToken); // a call only when the consumer asks for more
            if (orders.Count == 0) { yield break; }
            foreach (var order in orders)
            {
                yield return order;
            }
        }
    }
}
