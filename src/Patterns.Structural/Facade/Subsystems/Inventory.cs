using Patterns.Shop;

namespace Patterns.Structural.Facade.Subsystems;

/// <summary>
/// The stock of each product. A simulated subsystem, shared by every level: it does its own job and knows
/// nothing about checkouts or facades. Guide: §5.5.
/// </summary>
// Role: Subsystem — reserves stock.
// Guide: §5.5
public sealed class Inventory
{
    private readonly Dictionary<Guid, int> _stock;
    private readonly Dictionary<Guid, string> _names;

    public Inventory(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);
        var list = products.ToList();
        _stock = list.ToDictionary(p => p.Id, p => p.Stock);
        _names = list.ToDictionary(p => p.Id, p => p.Name);
    }

    public int StockOf(Guid productId) => _stock[productId];

    /// <summary>Reserves every line or none: the stock of all lines is checked before any is taken.</summary>
    public void Reserve(IEnumerable<OrderLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        var wanted = lines.GroupBy(line => line.Product.Id)
            .ToDictionary(group => group.Key, group => group.Sum(line => line.Quantity));

        foreach (var (productId, quantity) in wanted)
        {
            if (_stock.GetValueOrDefault(productId) < quantity)
            {
                var name = _names.GetValueOrDefault(productId, "an unknown product");
                throw new InvalidOperationException($"Not enough stock for {name}.");
            }
        }
        foreach (var (productId, quantity) in wanted)
        {
            _stock[productId] -= quantity;
        }
    }
}
