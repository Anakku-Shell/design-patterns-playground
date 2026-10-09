using Patterns.Shop;

namespace Patterns.Modern.UnitOfWork.Classic;

// Role: Store — the data; changed only by a commit.
// Guide: §7.4
public sealed class InMemoryShop
{
    private readonly List<Order> _orders = [];

    public InMemoryShop(IEnumerable<Product> products) => Stock = products.ToDictionary(p => p.Id, p => p.Stock);

    public Dictionary<Guid, int> Stock { get; }

    public IReadOnlyList<Order> Orders => _orders;

    public void AddOrder(Order order) => _orders.Add(order);
}

// Role: UnitOfWork — collects changes and applies them together, or not at all.
public sealed class UnitOfWork(InMemoryShop shop)
{
    private readonly List<Order> _newOrders = [];
    private readonly List<(Guid ProductId, int Units)> _stockChanges = [];

    public void RegisterOrder(Order order) => _newOrders.Add(order);

    public void DecreaseStock(Guid productId, int units)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(units); // a negative decrease would add stock
        _stockChanges.Add((productId, units));
    }

    public void Commit()
    {
        // 1. Validate everything first, per product: two changes that each fit may not fit together.
        foreach (var change in _stockChanges.GroupBy(c => c.ProductId))
        {
            if (!shop.Stock.TryGetValue(change.Key, out var inStock) || inStock < change.Sum(c => c.Units))
            {
                throw new InvalidOperationException($"Not enough stock for {change.Key}.");
            }
        }

        // 2. …then apply everything. Nothing above wrote anything, so a failure leaves the shop untouched.
        foreach (var (productId, units) in _stockChanges)
        {
            shop.Stock[productId] -= units;
        }
        foreach (var order in _newOrders)
        {
            shop.AddOrder(order);
        }
        _newOrders.Clear();
        _stockChanges.Clear();
    }
}

// Role: Client — registers the changes of one operation and commits once at the end.
public sealed class CheckoutService(InMemoryShop shop)
{
    public void Confirm(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var work = new UnitOfWork(shop);
        work.RegisterOrder(order);
        foreach (var line in order.Lines)
        {
            work.DecreaseStock(line.Product.Id, line.Quantity);
        }
        work.Commit();
    }
}
