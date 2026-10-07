using System.Globalization;
using Patterns.Shop;
using Patterns.Structural.Adapter.External;

namespace Patterns.Structural.Adapter.Classic;

// Role: Adapter — speaks the legacy carrier's text protocol so nobody else has to.
// Guide: §5.1
public sealed class LegacyCarrierAdapter(LegacyCarrierClient client) : IShippingQuoteProvider
{
    public ShippingQuote Quote(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        var cents = (int)(order.Total * 100);
        var payload = string.Create(CultureInfo.InvariantCulture,
            $"{order.ShippingAddress.Country};{order.Units};{cents}");

        var fields = client.RequestQuote(payload)
            .Split(';')
            .Select(part => part.Split('='))
            .ToDictionary(pair => pair[0], pair => pair[1]);

        // InvariantCulture: the carrier always writes "4.00". Under a Spanish culture a plain
        // decimal.Parse would read "." as the thousands separator and return 400.
        return new ShippingQuote(
            decimal.Parse(fields["PRICE"], CultureInfo.InvariantCulture),
            int.Parse(fields["DAYS"], CultureInfo.InvariantCulture));
    }
}
