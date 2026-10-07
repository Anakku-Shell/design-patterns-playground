using System.Globalization;
using Patterns.Shop;
using Patterns.Structural.Adapter.External;

namespace Patterns.Structural.Adapter.Problem;

// Guide: §5.1
public sealed class OrderTracking(LegacyCarrierClient carrier)
{
    public int DeliveryDays(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        // PAIN: a second copy of the payload and the parsing, written slightly differently from the one in
        // CheckoutSummary. Fix one, forget the other.
        var payload = string.Create(CultureInfo.InvariantCulture,
            $"{order.ShippingAddress.Country};{order.Units};{(int)(order.Total * 100)}");
        var answer = carrier.RequestQuote(payload);
        var daysField = answer.Split(';').Single(part => part.StartsWith("DAYS=", StringComparison.Ordinal));
        return int.Parse(daysField[5..], CultureInfo.InvariantCulture);
    }
}
