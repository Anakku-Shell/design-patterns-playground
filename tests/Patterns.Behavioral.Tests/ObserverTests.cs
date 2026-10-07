using Patterns.Behavioral.Observer.Classic;
using Patterns.Behavioral.Observer.DotNet;
using Patterns.Shop;
using Xunit;
using Problem = Patterns.Behavioral.Observer.Problem;

namespace Patterns.Behavioral.Tests;

public sealed class ObserverTests
{
    private static readonly Order TwoBooks =
        SampleData.OrderOf((SampleData.Book, 2)) with { Id = new Guid("1a2b3c4d-0000-0000-0000-000000000000") };

    private static readonly string[] ThreeReactions =
    [
        "email: order 1a2b3c4d confirmed",
        "stock: reserve 2 units",
        "analytics: order total 25.00",
    ];

    private sealed class FailingObserver : IOrderObserver
    {
        public void OnOrderPlaced(Order order) => throw new InvalidOperationException("stock service down");
    }

    private sealed class SelfDetachingObserver(OrderPublisher publisher, ICollection<string> log) : IOrderObserver
    {
        public void OnOrderPlaced(Order order)
        {
            log.Add("once");
            publisher.Detach(this);
        }
    }

    [Fact]
    public void AllLevels_ReactInSubscriptionOrder()
    {
        var problemLog = new List<string>();
        new Problem.OrderService(
            new Problem.EmailSender(problemLog), new Problem.StockUpdater(problemLog), new Problem.Analytics(problemLog))
            .Place(TwoBooks);

        var classicLog = new List<string>();
        var publisher = new OrderPublisher();
        publisher.Attach(new EmailObserver(classicLog));
        publisher.Attach(new StockObserver(classicLog));
        publisher.Attach(new AnalyticsObserver(classicLog));
        publisher.Publish(TwoBooks);

        var eventLog = new List<string>();
        var service = new OrderService();
        service.OrderPlaced += (_, e) => eventLog.Add(Reactions.Email(e.Order));
        service.OrderPlaced += (_, e) => eventLog.Add(Reactions.Stock(e.Order));
        service.OrderPlaced += (_, e) => eventLog.Add(Reactions.Analytics(e.Order));
        service.Place(TwoBooks);

        var feedLog = new List<string>();
        var feed = new OrderFeed();
        using (feed.Subscribe(new LoggingObserver(feedLog, Reactions.Email)))
        using (feed.Subscribe(new LoggingObserver(feedLog, Reactions.Stock)))
        using (feed.Subscribe(new LoggingObserver(feedLog, Reactions.Analytics)))
        {
            feed.Publish(TwoBooks);
        }

        Assert.Equal(ThreeReactions, problemLog);
        Assert.Equal(ThreeReactions, classicLog);
        Assert.Equal(ThreeReactions, eventLog);
        Assert.Equal(ThreeReactions, feedLog);
    }

    [Fact]
    public void Reactions_IgnoreTheCurrentCulture()
    {
        var line = CultureScope.WithDecimalComma(() => Reactions.Analytics(TwoBooks));
        var classicLog = new List<string>();
        CultureScope.WithDecimalComma(() =>
        {
            new AnalyticsObserver(classicLog).OnOrderPlaced(TwoBooks);
            return 0;
        });

        Assert.Equal("analytics: order total 25.00", line);
        Assert.Equal(["analytics: order total 25.00"], classicLog);
    }

    [Fact]
    public void Detached_IsNotNotified()
    {
        var log = new List<string>();
        var publisher = new OrderPublisher();
        var email = new EmailObserver(log);
        publisher.Attach(email);
        publisher.Attach(new StockObserver(log));

        publisher.Detach(email);
        publisher.Publish(TwoBooks);

        Assert.Equal(["stock: reserve 2 units"], log);
    }

    [Fact]
    public void Event_Unsubscribed_IsNotNotified()
    {
        var log = new List<string>();
        var service = new OrderService();
        EventHandler<OrderPlacedEventArgs> email = (_, e) => log.Add(Reactions.Email(e.Order));
        service.OrderPlaced += email;

        service.OrderPlaced -= email; // the same delegate instance
        service.Place(TwoBooks);

        Assert.Empty(log);
    }

    [Fact]
    public void Classic_FailingObserver_DoesNotStopTheOthers()
    {
        var log = new List<string>();
        var publisher = new OrderPublisher();
        publisher.Attach(new EmailObserver(log));
        publisher.Attach(new FailingObserver());
        publisher.Attach(new AnalyticsObserver(log));

        var error = Assert.Throws<AggregateException>(() => publisher.Publish(TwoBooks));

        Assert.Equal(["email: order 1a2b3c4d confirmed", "analytics: order total 25.00"], log);
        Assert.Equal("stock service down", Assert.Single(error.InnerExceptions).Message);
    }

    [Fact]
    public void Classic_ObserverMayDetachWhileNotified()
    {
        var log = new List<string>();
        var publisher = new OrderPublisher();
        publisher.Attach(new SelfDetachingObserver(publisher, log));
        publisher.Attach(new StockObserver(log));

        publisher.Publish(TwoBooks);
        publisher.Publish(TwoBooks);

        Assert.Equal(["once", "stock: reserve 2 units", "stock: reserve 2 units"], log);
    }

    [Fact]
    public void Event_StopsAtFirstFailingSubscriber()
    {
        var log = new List<string>();
        var service = new OrderService();
        service.OrderPlaced += (_, e) => log.Add(Reactions.Email(e.Order));
        service.OrderPlaced += (_, _) => throw new InvalidOperationException("stock service down");
        service.OrderPlaced += (_, e) => log.Add(Reactions.Analytics(e.Order));

        var error = Assert.Throws<InvalidOperationException>(() => service.Place(TwoBooks));

        Assert.Equal("stock service down", error.Message);
        Assert.Equal(["email: order 1a2b3c4d confirmed"], log); // analytics never ran
    }

    [Fact]
    public void Observable_DisposingTwice_LeavesTheOtherSubscription()
    {
        var log = new List<string>();
        var feed = new OrderFeed();
        var observer = new LoggingObserver(log, Reactions.Stock);
        var first = feed.Subscribe(observer);
        using var second = feed.Subscribe(observer);

        first.Dispose();
        first.Dispose();
        feed.Publish(TwoBooks);

        Assert.Equal(["stock: reserve 2 units"], log);
    }

    [Fact]
    public void Classic_PublishNull_Throws()
    {
        var publisher = new OrderPublisher();
        publisher.Attach(new StockObserver([]));

        Assert.Throws<ArgumentNullException>(() => publisher.Publish(null!));
    }

    [Fact]
    public void Observable_DisposeUnsubscribes()
    {
        var log = new List<string>();
        var feed = new OrderFeed();
        var subscription = feed.Subscribe(new LoggingObserver(log, Reactions.Stock));
        feed.Publish(TwoBooks);

        subscription.Dispose();
        feed.Publish(TwoBooks);
        subscription.Dispose(); // twice is harmless

        Assert.Equal(["stock: reserve 2 units"], log);
    }
}
