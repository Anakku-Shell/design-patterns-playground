namespace Patterns.Shop;

/// <summary>Where an order is in its life cycle (the state diagram of guide §3.8, implemented in §6.8).</summary>
public enum OrderStatus
{
    Draft,
    Placed,
    Paid,
    Shipped,
    Cancelled,
}
