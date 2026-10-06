using Patterns.Shop;

namespace Patterns.Creational.Builder.Problem;

// Guide: §4.4
public sealed class MutableOrder
{
    public Customer? Customer { get; set; }
    public Address? ShippingAddress { get; set; }
    public IList<OrderLine> Lines { get; } = new List<OrderLine>();

    public decimal Total => Lines.Sum(line => line.LineTotal);

    // PAIN: an invalid order exists until someone remembers to call IsValid(), and every caller must
    // remember to. Rules such as "the same product twice adds up" are left to each caller.
    public bool IsValid() => Customer is not null && ShippingAddress is not null && Lines.Count > 0;
}
