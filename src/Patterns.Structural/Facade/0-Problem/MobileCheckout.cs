using Patterns.Shop;
using Patterns.Structural.Facade.Subsystems;

namespace Patterns.Structural.Facade.Problem;

// Guide: §5.5
public sealed class MobileCheckout(Inventory inventory, PaymentGateway payments, Shipping shipping, Mailer mailer)
{
    public (string PaymentId, string TrackingNumber) Place(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        // PAIN: the same four calls, in the same order, as WebCheckout. Today the copies agree; nothing
        // keeps them that way.
        inventory.Reserve(order.Lines);
        var paymentId = payments.Charge(order.Customer, order.Total);
        var tracking = shipping.Schedule(order);
        mailer.Send(order.Customer, $"Order confirmed. Tracking number: {tracking}.");
        return (paymentId, tracking);
    }
}
