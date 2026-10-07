using Patterns.Shop;

namespace Patterns.Behavioral.State.DotNet;

/// <summary>What can happen to an order. Guide: §6.8.</summary>
public enum OrderAction
{
    Place,
    Pay,
    Ship,
    Cancel,
}

/// <summary>
/// The whole state diagram as one transition table: a <c>switch</c> expression on (status, action).
/// Enough when states differ only in which moves they allow. Guide: §6.8.
/// </summary>
public static class OrderTransitions
{
    public static OrderStatus Next(OrderStatus from, OrderAction action) => (from, action) switch
    {
        (OrderStatus.Draft, OrderAction.Place) => OrderStatus.Placed,
        (OrderStatus.Placed, OrderAction.Pay) => OrderStatus.Paid,
        (OrderStatus.Paid, OrderAction.Ship) => OrderStatus.Shipped,
        (OrderStatus.Draft or OrderStatus.Placed or OrderStatus.Paid, OrderAction.Cancel) => OrderStatus.Cancelled,
        _ => throw new InvalidOperationException(
            $"Cannot {action.ToString().ToLowerInvariant()} an order that is {from}."),
    };
}
