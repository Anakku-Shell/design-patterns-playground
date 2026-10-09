using Patterns.Shop;
using Patterns.Structural.Facade.Subsystems;

namespace Patterns.Structural.Facade.Classic;

/// <summary>What placing an order gives back.</summary>
public sealed record CheckoutReceipt(Guid OrderId, string PaymentId, string TrackingNumber);

// Role: Facade — one call to place an order; the order of the steps lives only here.
// Guide: §5.5
public sealed class CheckoutFacade(Inventory inventory, PaymentGateway payments, Shipping shipping, Mailer mailer)
{
    public CheckoutReceipt PlaceOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        inventory.Reserve(order.Lines); // all-or-nothing, and before charging: no money taken without stock
        var paymentId = payments.Charge(order.Customer, order.Total);
        var tracking = shipping.Schedule(order);
        mailer.Send(order.Customer, $"Order confirmed. Tracking number: {tracking}.");
        return new CheckoutReceipt(order.Id, paymentId, tracking);
    }
}
