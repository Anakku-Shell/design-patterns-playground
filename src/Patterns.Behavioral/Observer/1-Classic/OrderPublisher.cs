using System.Globalization;
using Patterns.Shop;

namespace Patterns.Behavioral.Observer.Classic;

// Role: Observer — anything that wants to know when an order is placed.
// Guide: §6.7
public interface IOrderObserver
{
    void OnOrderPlaced(Order order);
}

// Role: Subject — announces placed orders to whoever subscribed; knows only the interface.
public sealed class OrderPublisher
{
    private readonly List<IOrderObserver> _observers = [];

    public void Attach(IOrderObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        _observers.Add(observer);
    }

    public void Detach(IOrderObserver observer) => _observers.Remove(observer);

    public void Publish(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        // One failing observer must not silence the others: notify everybody, then report.
        var failures = new List<Exception>();
        foreach (var observer in _observers.ToList()) // a copy: an observer may detach while notified
        {
            try { observer.OnOrderPlaced(order); }
            catch (Exception ex) { failures.Add(ex); }
        }
        if (failures.Count > 0) { throw new AggregateException(failures); }
    }
}

// Role: ConcreteObserver — the confirmation email.
public sealed class EmailObserver(ICollection<string> log) : IOrderObserver
{
    public void OnOrderPlaced(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        log.Add($"email: order {order.Id.ToString()[..8]} confirmed");
    }
}

// Role: ConcreteObserver — the stock reservation.
public sealed class StockObserver(ICollection<string> log) : IOrderObserver
{
    public void OnOrderPlaced(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        log.Add(string.Create(CultureInfo.InvariantCulture, $"stock: reserve {order.Units} units"));
    }
}

// Role: ConcreteObserver — the analytics event.
public sealed class AnalyticsObserver(ICollection<string> log) : IOrderObserver
{
    public void OnOrderPlaced(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        log.Add(string.Create(CultureInfo.InvariantCulture, $"analytics: order total {order.Total:0.00}"));
    }
}
