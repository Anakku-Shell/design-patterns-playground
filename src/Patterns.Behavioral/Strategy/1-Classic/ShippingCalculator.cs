using Patterns.Shop;

namespace Patterns.Behavioral.Strategy.Classic;

// Role: Context — prices shipping with whatever strategy it was given; does not know which one.
// Guide: §6.9
public sealed class ShippingCalculator(IShippingStrategy strategy)
{
    public decimal CostFor(Order order) => strategy.CostFor(order);
}
