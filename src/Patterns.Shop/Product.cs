namespace Patterns.Shop;

/// <summary>Something the shop sells. <c>Sku</c> is the shop's own product code (BOOK-001). Guide: §3.8.</summary>
public sealed record Product(Guid Id, string Name, string Sku, decimal Price, Category Category, int Stock);
