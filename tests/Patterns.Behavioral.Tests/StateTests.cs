using Patterns.Behavioral.State.Classic;
using Patterns.Behavioral.State.DotNet;
using Patterns.Shop;
using Xunit;
using Problem = Patterns.Behavioral.State.Problem;

namespace Patterns.Behavioral.Tests;

public sealed class StateTests
{
    // How to bring a new (Draft) order to each status with legal moves only.
    private static OrderAction[] PathTo(OrderStatus status) => status switch
    {
        OrderStatus.Draft => [],
        OrderStatus.Placed => [OrderAction.Place],
        OrderStatus.Paid => [OrderAction.Place, OrderAction.Pay],
        OrderStatus.Shipped => [OrderAction.Place, OrderAction.Pay, OrderAction.Ship],
        OrderStatus.Cancelled => [OrderAction.Cancel],
        _ => throw new ArgumentOutOfRangeException(nameof(status)),
    };

    private static void Apply(Problem.OrderWorkflow order, OrderAction action)
    {
        switch (action)
        {
            case OrderAction.Place: order.Place(); break;
            case OrderAction.Pay: order.Pay(); break;
            case OrderAction.Ship: order.Ship(); break;
            case OrderAction.Cancel: order.Cancel(); break;
        }
    }

    private static void Apply(OrderContext order, OrderAction action)
    {
        switch (action)
        {
            case OrderAction.Place: order.Place(); break;
            case OrderAction.Pay: order.Pay(); break;
            case OrderAction.Ship: order.Ship(); break;
            case OrderAction.Cancel: order.Cancel(); break;
        }
    }

    // The new status, or the message of the exception.
    private static string Outcome(Func<OrderStatus> act)
    {
        try
        {
            return act().ToString();
        }
        catch (InvalidOperationException e)
        {
            return e.Message;
        }
    }

    [Theory]
    [InlineData(OrderStatus.Draft, OrderAction.Place, "Placed")]
    [InlineData(OrderStatus.Draft, OrderAction.Pay, "Cannot pay an order that is Draft.")]
    [InlineData(OrderStatus.Draft, OrderAction.Ship, "Cannot ship an order that is Draft.")]
    [InlineData(OrderStatus.Draft, OrderAction.Cancel, "Cancelled")]
    [InlineData(OrderStatus.Placed, OrderAction.Place, "Cannot place an order that is Placed.")]
    [InlineData(OrderStatus.Placed, OrderAction.Pay, "Paid")]
    [InlineData(OrderStatus.Placed, OrderAction.Ship, "Cannot ship an order that is Placed.")]
    [InlineData(OrderStatus.Placed, OrderAction.Cancel, "Cancelled")]
    [InlineData(OrderStatus.Paid, OrderAction.Place, "Cannot place an order that is Paid.")]
    [InlineData(OrderStatus.Paid, OrderAction.Pay, "Cannot pay an order that is Paid.")]
    [InlineData(OrderStatus.Paid, OrderAction.Ship, "Shipped")]
    [InlineData(OrderStatus.Paid, OrderAction.Cancel, "Cancelled")]
    [InlineData(OrderStatus.Shipped, OrderAction.Place, "Cannot place an order that is Shipped.")]
    [InlineData(OrderStatus.Shipped, OrderAction.Pay, "Cannot pay an order that is Shipped.")]
    [InlineData(OrderStatus.Shipped, OrderAction.Ship, "Cannot ship an order that is Shipped.")]
    [InlineData(OrderStatus.Shipped, OrderAction.Cancel, "Cannot cancel an order that is Shipped.")]
    [InlineData(OrderStatus.Cancelled, OrderAction.Place, "Cannot place an order that is Cancelled.")]
    [InlineData(OrderStatus.Cancelled, OrderAction.Pay, "Cannot pay an order that is Cancelled.")]
    [InlineData(OrderStatus.Cancelled, OrderAction.Ship, "Cannot ship an order that is Cancelled.")]
    [InlineData(OrderStatus.Cancelled, OrderAction.Cancel, "Cannot cancel an order that is Cancelled.")]
    public void AllLevels_FollowTheSameDiagram(OrderStatus from, OrderAction action, string expected)
    {
        var problem = new Problem.OrderWorkflow();
        var classic = new OrderContext();
        foreach (var step in PathTo(from))
        {
            Apply(problem, step);
            Apply(classic, step);
        }
        Assert.Equal(from, problem.Status);
        Assert.Equal(from, classic.Status);

        Assert.Equal(expected, Outcome(() => { Apply(problem, action); return problem.Status; }));
        Assert.Equal(expected, Outcome(() => { Apply(classic, action); return classic.Status; }));
        Assert.Equal(expected, Outcome(() => OrderTransitions.Next(from, action)));
    }

    [Fact]
    public void HappyPath_DraftToShipped()
    {
        var order = new OrderContext();

        order.Place();
        order.Pay();
        order.Ship();

        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    [Fact]
    public void CannotShipAPlacedOrder()
    {
        var order = new OrderContext();
        order.Place();

        var error = Assert.Throws<InvalidOperationException>(order.Ship);

        Assert.Equal("Cannot ship an order that is Placed.", error.Message);
        Assert.Equal(OrderStatus.Placed, order.Status); // a refused move changes nothing
    }

    [Fact]
    public void CannotCancelAShippedOrder()
    {
        var error = Assert.Throws<InvalidOperationException>(() => OrderTransitions.Next(OrderStatus.Shipped, OrderAction.Cancel));

        Assert.Equal("Cannot cancel an order that is Shipped.", error.Message);
    }

    [Fact]
    public void DotNet_WorksWithRecords()
    {
        var order = SampleData.OrderOf((SampleData.Book, 1)) with { Status = OrderStatus.Placed };

        var paid = order with { Status = OrderTransitions.Next(order.Status, OrderAction.Pay) };

        Assert.Equal(OrderStatus.Paid, paid.Status);
        Assert.Equal(OrderStatus.Placed, order.Status);
    }
}
