using Patterns.Shop;

namespace Patterns.Behavioral.Strategy.Classic;

// Role: Strategy — one way of pricing shipping.
// Guide: §6.9
public interface IShippingStrategy
{
    decimal CostFor(Order order);
}

// Role: ConcreteStrategy — standard shipping, free from 50.00 (inclusive).
public sealed class StandardShipping : IShippingStrategy
{
    public decimal CostFor(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return order.Total >= 50.00m ? 0.00m : 4.99m;
    }
}

// Role: ConcreteStrategy — express: a fixed fee plus one euro per unit.
public sealed class ExpressShipping : IShippingStrategy
{
    public decimal CostFor(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return 9.99m + 1.00m * order.Units;
    }
}

// Role: ConcreteStrategy — collected in the store, free.
public sealed class StorePickup : IShippingStrategy
{
    public decimal CostFor(Order order) => 0.00m;
}
