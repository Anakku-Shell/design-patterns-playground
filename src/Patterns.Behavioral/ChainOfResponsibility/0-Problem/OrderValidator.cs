using Patterns.Shop;

namespace Patterns.Behavioral.ChainOfResponsibility.Problem;

// Guide: §6.1
public sealed class OrderValidator
{
    // PAIN: one method knows every rule and their order. Adding, removing or reordering a rule
    // means editing (and re-testing) this method.
    public OrderCheck Validate(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order.Lines.Count == 0) { return OrderCheck.Fail("Order has no lines."); }
        if (order.Lines.Any(l => l.Quantity > 10)) { return OrderCheck.Fail("At most 10 units per product."); }
        var missing = order.Lines.FirstOrDefault(l => l.Product.Stock < l.Quantity);
        if (missing is not null) { return OrderCheck.Fail($"Not enough stock for {missing.Product.Name}."); }
        if (order.ShippingAddress.Country is not ("ES" or "PT" or "FR"))
        {
            return OrderCheck.Fail($"We do not ship to {order.ShippingAddress.Country}.");
        }
        return OrderCheck.Ok;
    }
}
