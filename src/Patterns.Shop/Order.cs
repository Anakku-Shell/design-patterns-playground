namespace Patterns.Shop;

/// <summary>A customer's purchase. Totals are computed from the lines, so they can never disagree with them. Guide: §3.8.</summary>
public sealed record Order(
    Guid Id,
    Customer Customer,
    IReadOnlyList<OrderLine> Lines,
    Address ShippingAddress,
    OrderStatus Status = OrderStatus.Draft)
{
    public decimal Total => Lines.Sum(line => line.LineTotal);

    public int Units => Lines.Sum(line => line.Quantity);
}
