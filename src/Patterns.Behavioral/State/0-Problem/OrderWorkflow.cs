using Patterns.Shop;

namespace Patterns.Behavioral.State.Problem;

// Guide: §6.8
public sealed class OrderWorkflow
{
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;

    public void Place()
    {
        if (Status != OrderStatus.Draft) { throw Illegal("place"); }
        Status = OrderStatus.Placed;
    }

    public void Pay()
    {
        // PAIN: the rules of each status are scattered across four methods. To know everything a
        // Paid order can do, you read all of them.
        if (Status != OrderStatus.Placed) { throw Illegal("pay"); }
        Status = OrderStatus.Paid;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Paid) { throw Illegal("ship"); }
        Status = OrderStatus.Shipped;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Shipped or OrderStatus.Cancelled) { throw Illegal("cancel"); }
        Status = OrderStatus.Cancelled;
    }

    private InvalidOperationException Illegal(string action) =>
        new($"Cannot {action} an order that is {Status}.");
}
