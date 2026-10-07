using Patterns.Shop;

namespace Patterns.Behavioral.State.Classic;

// Role: State — what an order can do in one status. By default, nothing is allowed.
// Guide: §6.8
public abstract class OrderState
{
    public abstract OrderStatus Status { get; }

    public virtual void Place(OrderContext order) => throw Illegal("place");

    public virtual void Pay(OrderContext order) => throw Illegal("pay");

    public virtual void Ship(OrderContext order) => throw Illegal("ship");

    public virtual void Cancel(OrderContext order) => throw Illegal("cancel");

    private InvalidOperationException Illegal(string action) =>
        new($"Cannot {action} an order that is {Status}.");
}

// Role: ConcreteState — a draft can be placed or cancelled.
public sealed class DraftState : OrderState
{
    public override OrderStatus Status => OrderStatus.Draft;

    public override void Place(OrderContext order) => order.TransitionTo(new PlacedState());

    public override void Cancel(OrderContext order) => order.TransitionTo(new CancelledState());
}

// Role: ConcreteState — a placed order can be paid or cancelled, nothing else.
public sealed class PlacedState : OrderState
{
    public override OrderStatus Status => OrderStatus.Placed;

    public override void Pay(OrderContext order) => order.TransitionTo(new PaidState());

    public override void Cancel(OrderContext order) => order.TransitionTo(new CancelledState());
}

// Role: ConcreteState — a paid order can be shipped or cancelled.
public sealed class PaidState : OrderState
{
    public override OrderStatus Status => OrderStatus.Paid;

    public override void Ship(OrderContext order) => order.TransitionTo(new ShippedState());

    public override void Cancel(OrderContext order) => order.TransitionTo(new CancelledState());
}

// Role: ConcreteState — the end of the road: overrides nothing, so every action throws.
public sealed class ShippedState : OrderState
{
    public override OrderStatus Status => OrderStatus.Shipped;
}

// Role: ConcreteState — the other end of the road.
public sealed class CancelledState : OrderState
{
    public override OrderStatus Status => OrderStatus.Cancelled;
}

// Role: Context — the order clients use; its behaviour comes from the current state object.
public sealed class OrderContext
{
    private OrderState _state = new DraftState();

    public OrderStatus Status => _state.Status;

    public void Place() => _state.Place(this);

    public void Pay() => _state.Pay(this);

    public void Ship() => _state.Ship(this);

    public void Cancel() => _state.Cancel(this);

    internal void TransitionTo(OrderState next) => _state = next;
}
