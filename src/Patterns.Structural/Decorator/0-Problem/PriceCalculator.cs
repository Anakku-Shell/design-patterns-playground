using Patterns.Shop;

namespace Patterns.Structural.Decorator.Problem;

// Guide: §5.4
public sealed class PriceCalculator
{
    // PAIN: each new rule adds a parameter and another branch; the order of the steps is hidden in
    // here, and there is no way to choose a different order.
    public decimal Price(Order order, bool applyVat, decimal? coupon)
    {
        ArgumentNullException.ThrowIfNull(order);

        var price = order.Total;
        if (applyVat) { price = decimal.Round(price * 1.21m, 2, MidpointRounding.AwayFromZero); }
        if (coupon is { } amount) { price = Math.Max(0, price - amount); }
        return price;
    }
}
