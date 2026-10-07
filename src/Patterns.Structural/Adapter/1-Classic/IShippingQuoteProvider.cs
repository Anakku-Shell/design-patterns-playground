using Patterns.Shop;

namespace Patterns.Structural.Adapter.Classic;

/// <summary>What shipping an order costs and how many days it takes, in our own types.</summary>
public sealed record ShippingQuote(decimal Price, int Days);

// Role: Target — what our code needs from a carrier, in our own terms (a "port").
// Guide: §5.1
public interface IShippingQuoteProvider
{
    ShippingQuote Quote(Order order);
}
