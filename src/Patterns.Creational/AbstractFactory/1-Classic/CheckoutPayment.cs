using Patterns.Shop;

namespace Patterns.Creational.AbstractFactory.Classic;

// Role: Client — knows the interfaces only; the family is decided by whoever passes the factory.
// Guide: §4.3
public sealed class CheckoutPayment(IPaymentProviderFactory provider)
{
    // Created once: the charger numbers its transactions (CARD-0001, CARD-0002…).
    private readonly IPaymentCharger _charger = provider.CreateCharger();
    private readonly IReceiptFormatter _formatter = provider.CreateReceiptFormatter();

    public string Pay(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var transactionId = _charger.Charge(order.Total);
        return _formatter.Format(transactionId, order.Total);
    }
}
