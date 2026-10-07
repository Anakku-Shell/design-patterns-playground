using Patterns.Shop;

namespace Patterns.Structural.Decorator.Classic;

// Role: Component — anything that can price an order.
// Guide: §5.4
public interface IPriceCalculator
{
    decimal PriceOf(Order order);
}

// Role: ConcreteComponent — the undecorated price, at the centre of every chain.
public sealed class BasePrice : IPriceCalculator
{
    public decimal PriceOf(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return order.Total;
    }
}

// Role: ConcreteDecorator — adds VAT to whatever the inner calculator says.
public sealed class VatDecorator(IPriceCalculator inner, decimal rate) : IPriceCalculator
{
    public decimal PriceOf(Order order) =>
        decimal.Round(inner.PriceOf(order) * (1 + rate), 2, MidpointRounding.AwayFromZero);
}

// Role: ConcreteDecorator — subtracts a coupon, never going below zero.
public sealed class CouponDecorator(IPriceCalculator inner, decimal amount) : IPriceCalculator
{
    public decimal PriceOf(Order order) =>
        decimal.Round(Math.Max(0, inner.PriceOf(order) - amount), 2, MidpointRounding.AwayFromZero);
}
