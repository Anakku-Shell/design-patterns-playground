using System.Globalization;
using Patterns.Shop;

namespace Patterns.Behavioral.Observer.DotNet;

/// <summary>What the event carries: push style, the observer receives the order itself. Guide: §6.7.</summary>
public sealed class OrderPlacedEventArgs(Order order) : EventArgs
{
    public Order Order { get; } = order;
}

// Role: Subject — a C# event is the subscriber list (a multicast delegate); only this class can raise it.
// Guide: §6.7
public sealed class OrderService
{
    public event EventHandler<OrderPlacedEventArgs>? OrderPlaced;

    // Invoke calls the handlers one after another: if one throws, the rest never run.
    public void Place(Order order) => OrderPlaced?.Invoke(this, new OrderPlacedEventArgs(order));
}

// Role: Subject — the BCL's interface for a stream of notifications; Subscribe returns the way out.
public sealed class OrderFeed : IObservable<Order>
{
    private readonly List<IObserver<Order>> _observers = [];

    public IDisposable Subscribe(IObserver<Order> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        _observers.Add(observer);
        return new Unsubscriber(() => _observers.Remove(observer)); // Dispose = unsubscribe
    }

    public void Publish(Order order)
    {
        foreach (var observer in _observers.ToList())
        {
            observer.OnNext(order);
        }
    }

    private sealed class Unsubscriber(Action unsubscribe) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) { return; } // a second Dispose must not remove another subscription
            _disposed = true;
            unsubscribe();
        }
    }
}

// Role: ConcreteObserver — writes one line per order; what it writes is a function.
public sealed class LoggingObserver(ICollection<string> log, Func<Order, string> describe) : IObserver<Order>
{
    public void OnNext(Order value) => log.Add(describe(value));

    public void OnError(Exception error) => log.Add($"error: {error?.Message}");

    public void OnCompleted() => log.Add("completed");
}

/// <summary>The three reactions to a placed order, as the lines they write.</summary>
public static class Reactions
{
    public static string Email(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return $"email: order {order.Id.ToString()[..8]} confirmed";
    }

    public static string Stock(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return string.Create(CultureInfo.InvariantCulture, $"stock: reserve {order.Units} units");
    }

    public static string Analytics(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return string.Create(CultureInfo.InvariantCulture, $"analytics: order total {order.Total:0.00}");
    }
}
