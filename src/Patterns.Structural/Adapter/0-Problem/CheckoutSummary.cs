using System.Globalization;
using Patterns.Shop;
using Patterns.Structural.Adapter.External;

namespace Patterns.Structural.Adapter.Problem;

// Guide: §5.1
public sealed class CheckoutSummary(LegacyCarrierClient carrier)
{
    public string Describe(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        // PAIN: our code speaks the carrier's private format. The same build-and-parse code lives in
        // OrderTracking too, and both break when the carrier changes its format.
        var cents = (int)(order.Total * 100);
        var answer = carrier.RequestQuote(
            string.Create(CultureInfo.InvariantCulture, $"{order.ShippingAddress.Country};{order.Units};{cents}"));
        var parts = answer.Split(';');
        var price = decimal.Parse(parts[0]["PRICE=".Length..], CultureInfo.InvariantCulture);
        var days = int.Parse(parts[1]["DAYS=".Length..], CultureInfo.InvariantCulture);
        return string.Create(CultureInfo.InvariantCulture, $"Shipping {price:0.00} in {days} days");
    }
}
