namespace Patterns.Shop;

/// <summary>One product in an order and how many units. Guide: §3.8.</summary>
public sealed record OrderLine(Product Product, int Quantity)
{
    public decimal LineTotal => Product.Price * Quantity;
}
