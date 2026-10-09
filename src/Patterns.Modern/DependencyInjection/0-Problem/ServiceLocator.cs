using System.Collections.Concurrent;
using System.Globalization;
using Patterns.Shop;

namespace Patterns.Modern.DependencyInjection.Problem;

/// <summary>A global registry that classes ask for what they need. Often sold as the fix; it is an anti-pattern.</summary>
// Guide: §7.1
public static class ServiceLocator
{
    private static readonly ConcurrentDictionary<Type, object> Services = new();

    public static void Register<T>(T service) where T : class => Services[typeof(T)] = service;

    public static void Clear() => Services.Clear();

    public static T Get<T>() where T : class =>
        Services.TryGetValue(typeof(T), out var service)
            ? (T)service
            : throw new InvalidOperationException($"No service registered for {typeof(T).Name}.");
}

public sealed class LocatorCheckoutService
{
    public string Confirm(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        // PAIN: the dependencies are invisible from outside. The class can be constructed, and fails here,
        // at run time, if nobody registered a sender; you learn what it needs only by reading every method.
        var sender = ServiceLocator.Get<IEmailSender>();
        var clock = ServiceLocator.Get<TimeProvider>();
        var text = string.Create(CultureInfo.InvariantCulture,
            $"Order {order.Id.ToString()[..8]} confirmed at {clock.GetUtcNow():HH:mm}");
        sender.Send(order.Customer.Email, text);
        return text;
    }
}
