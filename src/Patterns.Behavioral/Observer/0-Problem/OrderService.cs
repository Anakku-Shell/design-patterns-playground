using System.Globalization;
using Patterns.Shop;

namespace Patterns.Behavioral.Observer.Problem;

// Guide: §6.7
public sealed class OrderService(EmailSender email, StockUpdater stock, Analytics analytics)
{
    public void Place(Order order)
    {
        // PAIN: OrderService depends on every class that reacts to an order. A new reaction means
        // editing this class, its constructor and its tests.
        email.SendConfirmation(order);
        stock.Reserve(order);
        analytics.Track(order);
    }
}

/// <summary>Sends the confirmation (here, writes a line to the log).</summary>
public sealed class EmailSender(ICollection<string> log)
{
    public void SendConfirmation(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        log.Add($"email: order {order.Id.ToString()[..8]} confirmed");
    }
}

/// <summary>Reserves the units of the order.</summary>
public sealed class StockUpdater(ICollection<string> log)
{
    public void Reserve(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        log.Add(string.Create(CultureInfo.InvariantCulture, $"stock: reserve {order.Units} units"));
    }
}

/// <summary>Records the sale.</summary>
public sealed class Analytics(ICollection<string> log)
{
    public void Track(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        log.Add(string.Create(CultureInfo.InvariantCulture, $"analytics: order total {order.Total:0.00}"));
    }
}
