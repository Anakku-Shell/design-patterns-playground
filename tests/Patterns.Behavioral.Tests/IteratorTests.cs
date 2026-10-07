using Patterns.Behavioral.Iterator.DotNet;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Behavioral.Iterator.Classic;
using DotNet = Patterns.Behavioral.Iterator.DotNet;
using Problem = Patterns.Behavioral.Iterator.Problem;

namespace Patterns.Behavioral.Tests;

public sealed class IteratorTests
{
    private static List<Order> Orders(int count) =>
        [.. Enumerable.Range(0, count).Select(_ => SampleData.OrderOf((SampleData.Book, 1)))];

    private static List<int> ProblemPageSizes(IEnumerable<Order> orders, int pageSize)
    {
        var history = new Problem.OrderHistory();
        history.Orders.AddRange(orders);
        return [.. new Problem.HistoryScreen().Pages(history, pageSize).Select(p => p.Count)];
    }

    private static List<int> ClassicPageSizes(IEnumerable<Order> orders, int pageSize)
    {
        var pages = new Classic.OrderHistory(orders).Pages(pageSize);
        var sizes = new List<int>();
        while (pages.HasNext)
        {
            sizes.Add(pages.GetNext().Count);
        }
        return sizes;
    }

    private static List<int> DotNetPageSizes(IEnumerable<Order> orders, int pageSize) =>
        [.. new DotNet.OrderHistory(orders).Pages(pageSize).Select(p => p.Count)];

    [Theory]
    [InlineData(7, 3, new[] { 3, 3, 1 })]
    [InlineData(6, 3, new[] { 3, 3 })]
    [InlineData(2, 5, new[] { 2 })]
    [InlineData(3, 1, new[] { 1, 1, 1 })]
    public void AllLevels_GiveTheSamePages(int orders, int pageSize, int[] expected)
    {
        var source = Orders(orders);

        Assert.Equal(expected, ProblemPageSizes(source, pageSize));
        Assert.Equal(expected, ClassicPageSizes(source, pageSize));
        Assert.Equal(expected, DotNetPageSizes(source, pageSize));
    }

    [Fact]
    public void Pages_KeepTheOrderOfTheHistory()
    {
        var source = Orders(7);

        Assert.Equal(source, new DotNet.OrderHistory(source).Pages(3).SelectMany(p => p));
        var pages = new Classic.OrderHistory(source).Pages(3);
        var all = new List<Order>();
        while (pages.HasNext)
        {
            all.AddRange(pages.GetNext());
        }
        Assert.Equal(source, all);
    }

    [Fact]
    public void EmptyHistory_HasNoPages()
    {
        Assert.Empty(ProblemPageSizes([], 3));
        Assert.False(new Classic.OrderHistory([]).Pages(3).HasNext);
        Assert.Empty(new DotNet.OrderHistory([]).Pages(3));
    }

    [Fact]
    public void PageSizeZero_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Problem.HistoryScreen().Pages(new Problem.OrderHistory(), 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Classic.OrderHistory([]).Pages(0));
        // Thrown by the call itself, not later at the first MoveNext: the check sits outside the iterator method.
        Assert.Throws<ArgumentOutOfRangeException>(() => new DotNet.OrderHistory([]).Pages(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new FakeOrderApi([], pageSize: 0));
    }

    [Fact]
    public void Classic_GetNextPastTheEnd_Throws()
    {
        var pages = new Classic.OrderHistory(Orders(2)).Pages(3);
        pages.GetNext();

        Assert.Equal("No more pages.", Assert.Throws<InvalidOperationException>(() => pages.GetNext()).Message);
    }

    [Fact]
    public void Yield_IsLazy()
    {
        var read = 0;
        IEnumerable<Order> CountingSource()
        {
            foreach (var order in Orders(7))
            {
                read++;
                yield return order;
            }
        }

        var pages = new DotNet.OrderHistory(CountingSource()).Pages(3);
        Assert.Equal(0, read); // nothing runs until someone enumerates

        var first = pages.First();

        Assert.Equal(3, first.Count);
        Assert.Equal(3, read); // one page read, never the rest
    }

    [Fact]
    public async Task AsyncStream_ReadsPagesOnDemand()
    {
        var api = new FakeOrderApi(Orders(7), pageSize: 3);
        var ct = TestContext.Current.CancellationToken;
        await using var orders = RemoteOrders.StreamAsync(api, ct).GetAsyncEnumerator(ct);
        Assert.Equal(0, api.Calls);

        var seen = 0;
        var callsAfter = new List<int>();
        while (await orders.MoveNextAsync())
        {
            seen++;
            callsAfter.Add(api.Calls);
        }

        Assert.Equal(7, seen);
        Assert.Equal([1, 1, 1, 2, 2, 2, 3], callsAfter); // a new page only when the consumer needs it
        Assert.Equal(4, api.Calls);                       // the fourth call got the empty page that ends the stream
    }

    [Fact]
    public async Task FakeApi_PagesAreOneBased()
    {
        var ct = TestContext.Current.CancellationToken;
        var api = new FakeOrderApi(Orders(4), pageSize: 3);

        Assert.Equal(3, (await api.GetPageAsync(1, ct)).Count);
        Assert.Single(await api.GetPageAsync(2, ct));
        Assert.Empty(await api.GetPageAsync(3, ct));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => api.GetPageAsync(0, ct));
    }
}
