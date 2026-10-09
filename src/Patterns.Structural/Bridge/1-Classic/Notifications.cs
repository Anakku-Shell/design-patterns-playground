using System.Globalization;
using Patterns.Shop;

namespace Patterns.Structural.Bridge.Classic;

// Role: Abstraction — what the shop wants to say, independent of how it travels.
// Guide: §5.2
public abstract class Notification(IMessageChannel channel)
{
    protected IMessageChannel Channel { get; } = channel; // the bridge to the other hierarchy

    public abstract string Send(Customer customer);
}

// Role: RefinedAbstraction — "your order has shipped".
public sealed class OrderShippedNotification(IMessageChannel channel, Guid orderId) : Notification(channel)
{
    public override string Send(Customer customer) =>
        Channel.Deliver(customer, "Order shipped", $"Order {orderId.ToString()[..8]} is on its way.");
}

// Role: RefinedAbstraction — "we could not charge you".
public sealed class PaymentFailedNotification(IMessageChannel channel, decimal amount) : Notification(channel)
{
    public override string Send(Customer customer) =>
        Channel.Deliver(customer, "Payment failed",
            string.Create(CultureInfo.InvariantCulture, $"We could not charge {amount:0.00}."));
}
