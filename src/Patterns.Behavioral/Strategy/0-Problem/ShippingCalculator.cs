using Patterns.Shop;

namespace Patterns.Behavioral.Strategy.Problem;

// Guide: §6.9
public sealed class ShippingCalculator
{
    public decimal CostFor(Order order, string method)
    {
        ArgumentNullException.ThrowIfNull(order);
        return method switch
        {
            "standard" => order.Total >= 50.00m ? 0.00m : 4.99m,
            "express" => 9.99m + 1.00m * order.Units,
            "pickup" => 0.00m,
            // PAIN: every new carrier means another case here, and this method's tests change with it.
            _ => throw new ArgumentException($"Unknown shipping method '{method}'."),
        };
    }
}
