using Patterns.Shop;

namespace Patterns.Behavioral.Mediator.Problem;

// Guide: §6.5
// PAIN: the endpoint knows every collaborator, and its constructor grows with every feature.
public sealed class OrdersEndpoint(OrderStore store, PriceCheck priceCheck, AuditLog audit)
{
    public Guid Place(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        priceCheck.Verify(order);
        store.Save(order);
        audit.Record($"placed {order.Id}");
        return order.Id;
    }

    public decimal TotalOf(Guid orderId) => store.Get(orderId).Total;
}
