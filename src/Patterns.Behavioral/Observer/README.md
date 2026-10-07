# Observer

**Relevance:** ⭐⭐⭐ Essential. C# `event`s, `IObservable<T>`, `IChangeToken` and every UI framework are built on it.

**Intent:** define a one-to-many dependency so that when one object changes, all its dependents are notified.

**Read first:** [`1-Classic/OrderPublisher.cs`](1-Classic/OrderPublisher.cs), then the tests `Classic_FailingObserver_DoesNotStopTheOthers` and `Event_StopsAtFirstFailingSubscriber`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `OrderService.Place` calls `EmailSender`, `StockUpdater` and `Analytics` directly: it depends on every reaction. |
| [`1-Classic/`](1-Classic/) | The subject `OrderPublisher` (`Attach`, `Detach`, `Publish`) and three `IOrderObserver`s; a failing observer does not stop the others (`AggregateException` at the end). |
| [`2-DotNet/`](2-DotNet/) | (a) `event EventHandler<OrderPlacedEventArgs>`: a failing handler stops the rest; (b) `OrderFeed : IObservable<Order>`, whose `Subscribe` returns an `IDisposable` that unsubscribes. |

**Run it:** `dotnet run --project src/Patterns.Runner -- observer`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.ObserverTests"`

**Guide:** [§6.7 Observer](../../../docs/DESIGN_PATTERNS_GUIDE.md#67-observer)
