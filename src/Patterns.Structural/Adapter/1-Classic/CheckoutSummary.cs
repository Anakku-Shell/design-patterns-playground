using System.Globalization;
using Patterns.Shop;

namespace Patterns.Structural.Adapter.Classic;

// Role: Client — knows only the target interface: no payloads, no parsing, no carrier SDK.
// Guide: §5.1
public sealed class CheckoutSummary(IShippingQuoteProvider shipping)
{
    public string Describe(Order order)
    {
        var quote = shipping.Quote(order);
        return string.Create(CultureInfo.InvariantCulture, $"Shipping {quote.Price:0.00} in {quote.Days} days");
    }
}
