using Patterns.Behavioral.State.Classic;
using Patterns.Behavioral.State.DotNet;
using Patterns.Demo;
using Patterns.Shop;

namespace Patterns.Behavioral.State;

public sealed class StateDemo : IDemo
{
    public string Key => "state";
    public string Name => "State";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§6.8";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(0, "Problem");
        narrator.Step("OrderWorkflow: Place, then Ship (each method checks Status with an if)");
        var workflow = new Problem.OrderWorkflow();
        workflow.Place();
        narrator.Result(Attempt(workflow.Ship, () => workflow.Status));

        narrator.Level(1, "Classic");
        var order = new OrderContext();
        narrator.Step("Place, Pay, Ship: each state object moves the order to the next one");
        order.Place();
        order.Pay();
        order.Ship();
        narrator.Result(order.Status.ToString());
        narrator.Step("Cancel a shipped order: ShippedState overrides nothing, the base throws");
        narrator.Result(Attempt(order.Cancel, () => order.Status));

        narrator.Level(2, ".NET");
        narrator.Step("OrderTransitions.Next(status, action): the diagram as one switch, used with a record");
        var placed = SampleData.OrderOf((SampleData.Book, 2)) with { Status = OrderStatus.Placed };
        var paid = placed with { Status = OrderTransitions.Next(placed.Status, OrderAction.Pay) };
        narrator.Result($"{placed.Status} → {paid.Status}");
        narrator.Step("Next(Placed, Ship)");
        narrator.Result(Attempt(() => OrderTransitions.Next(OrderStatus.Placed, OrderAction.Ship), () => OrderStatus.Placed));

        narrator.Takeaway("Put what each status allows in one place: a class per state when states have behaviour, a transition switch when they only allow moves.");
    }

    private static string Attempt(Action action, Func<OrderStatus> status)
    {
        try
        {
            action();
            return status().ToString();
        }
        catch (InvalidOperationException e)
        {
            return e.Message;
        }
    }

    private static string Attempt(Func<OrderStatus> action, Func<OrderStatus> status) =>
        Attempt(() => { action(); }, status);
}
