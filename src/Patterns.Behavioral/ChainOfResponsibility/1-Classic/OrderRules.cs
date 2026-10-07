using Patterns.Shop;

namespace Patterns.Behavioral.ChainOfResponsibility.Classic;

// Role: Handler — one rule in the chain; knows only the next link.
// Guide: §6.1
public abstract class OrderRule
{
    private OrderRule? _next;

    /// <summary>Returns the next rule, so a chain reads as a sentence: <c>a.SetNext(b).SetNext(c)</c>.</summary>
    public OrderRule SetNext(OrderRule next) => _next = next;

    public OrderCheck Check(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var result = Passes(order);
        if (!result.IsValid) { return result; }       // stop: first failure wins
        return _next?.Check(order) ?? OrderCheck.Ok;  // pass it on (or we were the last)
    }

    protected abstract OrderCheck Passes(Order order);
}

// Role: ConcreteHandler — refuses orders without lines.
public sealed class NotEmptyRule : OrderRule
{
    protected override OrderCheck Passes(Order order) =>
        order.Lines.Count == 0 ? OrderCheck.Fail("Order has no lines.") : OrderCheck.Ok;
}

// Role: ConcreteHandler — at most 10 units of any product.
public sealed class MaxQuantityRule : OrderRule
{
    protected override OrderCheck Passes(Order order) =>
        order.Lines.Any(l => l.Quantity > 10) ? OrderCheck.Fail("At most 10 units per product.") : OrderCheck.Ok;
}

// Role: ConcreteHandler — every line must be in stock.
public sealed class StockRule : OrderRule
{
    protected override OrderCheck Passes(Order order) =>
        order.Lines.FirstOrDefault(l => l.Product.Stock < l.Quantity) is { } missing
            ? OrderCheck.Fail($"Not enough stock for {missing.Product.Name}.")
            : OrderCheck.Ok;
}

// Role: ConcreteHandler — the shop ships to three countries.
public sealed class ShippingCountryRule : OrderRule
{
    protected override OrderCheck Passes(Order order) =>
        order.ShippingAddress.Country is "ES" or "PT" or "FR"
            ? OrderCheck.Ok
            : OrderCheck.Fail($"We do not ship to {order.ShippingAddress.Country}.");
}
