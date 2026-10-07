using Patterns.Behavioral.Observer.Classic;
using Patterns.Behavioral.Observer.DotNet;
using Patterns.Demo;
using Patterns.Shop;

namespace Patterns.Behavioral.Observer;

public sealed class ObserverDemo : IDemo
{
    public string Key => "observer";
    public string Name => "Observer";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Essential;
    public string GuideSection => "§6.7";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var order = SampleData.OrderOf((SampleData.Book, 2)) with { Id = new Guid("1a2b3c4d-0000-0000-0000-000000000000") }; // stable output

        narrator.Level(0, "Problem");
        narrator.Step("OrderService.Place calls EmailSender, StockUpdater and Analytics itself");
        var problemLog = new List<string>();
        new Problem.OrderService(
            new Problem.EmailSender(problemLog), new Problem.StockUpdater(problemLog), new Problem.Analytics(problemLog))
            .Place(order);
        Print(narrator, problemLog);

        narrator.Level(1, "Classic");
        narrator.Step("OrderPublisher with three attached observers; the stock one is failing today");
        var log = new List<string>();
        var publisher = new OrderPublisher();
        publisher.Attach(new EmailObserver(log));
        publisher.Attach(new FailingStock());
        publisher.Attach(new AnalyticsObserver(log));
        try
        {
            publisher.Publish(order);
        }
        catch (AggregateException e)
        {
            log.Add($"AggregateException with {e.InnerExceptions.Count} failure: {e.InnerExceptions[0].Message}");
        }
        Print(narrator, log);

        narrator.Level(2, ".NET");
        narrator.Step("event OrderPlaced: += three handlers, the second one throws");
        var eventLog = new List<string>();
        var service = new OrderService();
        service.OrderPlaced += (_, e) => eventLog.Add(Reactions.Email(e.Order));
        service.OrderPlaced += (_, _) => throw new InvalidOperationException("stock service down");
        service.OrderPlaced += (_, e) => eventLog.Add(Reactions.Analytics(e.Order));
        try
        {
            service.Place(order);
        }
        catch (InvalidOperationException e)
        {
            eventLog.Add($"{e.Message}: the analytics handler never ran");
        }
        Print(narrator, eventLog);
        narrator.Step("IObservable<Order>: Subscribe returns an IDisposable; disposing it unsubscribes");
        var feedLog = new List<string>();
        var feed = new OrderFeed();
        using (feed.Subscribe(new LoggingObserver(feedLog, Reactions.Stock)))
        {
            feed.Publish(order);
        }
        feed.Publish(order); // nobody listens any more
        Print(narrator, feedLog);

        narrator.Takeaway("The subject knows only an interface; new reactions subscribe without changing it. Unsubscribe, and decide what a failing observer does.");
    }

    private static void Print(Narrator narrator, IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            narrator.Result(line);
        }
    }

    private sealed class FailingStock : IOrderObserver
    {
        public void OnOrderPlaced(Order order) => throw new InvalidOperationException("stock service down");
    }
}
