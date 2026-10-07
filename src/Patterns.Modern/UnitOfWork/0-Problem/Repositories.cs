using Patterns.Shop;

namespace Patterns.Modern.UnitOfWork.Problem;

/// <summary>The data: stock per product and the saved orders. Stands in for two database tables.</summary>
// Guide: §7.4
public sealed class InMemoryShop
{
    private readonly List<Order> _orders = [];

    public InMemoryShop(IEnumerable<Product> products) => Stock = products.ToDictionary(p => p.Id, p => p.Stock);

    public Dictionary<Guid, int> Stock { get; }

    public IReadOnlyList<Order> Orders => _orders;

    public void AddOrder(Order order) => _orders.Add(order);
}

public sealed class OrderRepository(InMemoryShop shop)
{
    public void Save(Order order) => shop.AddOrder(order); // written now
}

public sealed class StockRepository(InMemoryShop shop)
{
    public void Decrease(Guid productId, int units)
    {
        if (!shop.Stock.TryGetValue(productId, out var inStock) || inStock < units)
        {
            throw new InvalidOperationException($"Not enough stock for {productId}.");
        }
        shop.Stock[productId] -= units; // written now
    }
}

public sealed class CheckoutService(OrderRepository orders, StockRepository stock)
{
    public void Confirm(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        // PAIN: each repository writes immediately. The order is saved and the books are taken, then the mug
        // is out of stock, and the shop is left with half-done work: an order and stock movements for a sale
        // that failed. Undoing it by hand means knowing everything that already happened.
        orders.Save(order);
        foreach (var line in order.Lines)
        {
            stock.Decrease(line.Product.Id, line.Quantity);
        }
    }
}
