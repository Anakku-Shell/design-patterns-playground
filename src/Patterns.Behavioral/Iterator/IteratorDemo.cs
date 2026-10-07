using Patterns.Behavioral.Iterator.DotNet;
using Patterns.Demo;
using Patterns.Shop;

namespace Patterns.Behavioral.Iterator;

public sealed class IteratorDemo : IDemo
{
    public string Key => "iterator";
    public string Name => "Iterator";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Essential;
    public string GuideSection => "§6.4";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var orders = Enumerable.Range(1, 7).Select(n => SampleData.OrderOf((SampleData.Book, n))).ToList();

        narrator.Level(0, "Problem");
        var problemHistory = new Problem.OrderHistory();
        problemHistory.Orders.AddRange(orders);
        narrator.Step("the screen pages history.Orders (a public List) with GetRange and Math.Min");
        narrator.Result(Sizes(new Problem.HistoryScreen().Pages(problemHistory, 3)));

        narrator.Level(1, "Classic");
        narrator.Step("while (pages.HasNext) pages.GetNext(): the list stays private");
        var pages = new Classic.OrderHistory(orders).Pages(3);
        var classicPages = new List<IReadOnlyList<Order>>();
        while (pages.HasNext)
        {
            classicPages.Add(pages.GetNext());
        }
        narrator.Result(Sizes(classicPages));

        narrator.Level(2, ".NET");
        narrator.Step("yield return: foreach over Pages(3), the compiler wrote the iterator");
        narrator.Result(Sizes([.. new DotNet.OrderHistory(orders).Pages(3)]));
        narrator.Step("await foreach over an API that returns pages of 3: calls made after each order");
        var api = new FakeOrderApi(orders, pageSize: 3);
        // IDemo.Run is synchronous; a console demo can wait for the result.
        var calls = CallsWhileStreaming(api).GetAwaiter().GetResult();
        narrator.Result($"{string.Join(", ", calls)} (then one more call gets the empty page: {api.Calls} calls)");

        narrator.Takeaway("Callers ask for the next element and never see the storage; in C#, yield return and IAsyncEnumerable write the iterator for you.");
    }

    private static string Sizes(IReadOnlyList<IReadOnlyList<Order>> pages) =>
        $"{pages.Count} pages of {string.Join(", ", pages.Select(p => p.Count))}";

    private static async Task<List<int>> CallsWhileStreaming(FakeOrderApi api)
    {
        var calls = new List<int>();
        await foreach (var _ in RemoteOrders.StreamAsync(api))
        {
            calls.Add(api.Calls);
        }
        return calls;
    }
}
